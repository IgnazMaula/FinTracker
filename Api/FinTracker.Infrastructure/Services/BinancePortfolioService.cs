using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using FinTracker.Application.Models;
using FinTracker.Application.Interfaces;
using FinTracker.Common.Shared.Model;
using Microsoft.Extensions.Configuration;
using Azure;
using System.Globalization;

namespace FinTracker.Infrastructure.Services;

public class BinancePortfolioService : IBinancePortfolioService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _apiSecret;

    public BinancePortfolioService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _apiKey = config["Binance:ApiKey"] ?? throw new Exception("Missing Binance ApiKey");
        _apiSecret = config["Binance:ApiSecret"] ?? throw new Exception("Missing Binance ApiSecret");
    }

    public async Task<BinancePortfolioModel> GetPortfolioAsync()
    {
        // 1. Get server time
        var timeResponse = await _httpClient.GetAsync("https://api.binance.com/api/v3/time");
        var serverTimeJson = await timeResponse.Content.ReadAsStringAsync();
        using var timeDoc = JsonDocument.Parse(serverTimeJson);
        var timestamp = timeDoc.RootElement.GetProperty("serverTime").GetInt64();
        var recvWindow = 60000;

        var queryStringBase = $"timestamp={timestamp}&recvWindow={recvWindow}";
        var signature = CreateSignature(queryStringBase);
        var authQuery = $"{queryStringBase}&signature={signature}";

        var portfolio = new BinancePortfolioModel();
        portfolio.Balances = new List<AssetBalanceModel>();
        decimal totalValue = 0;

        // 2. Get account balances
        var accountRequest = new HttpRequestMessage(HttpMethod.Get, $"https://api.binance.com/api/v3/account?{authQuery}");
        accountRequest.Headers.Add("X-MBX-APIKEY", _apiKey);
        var accountResponse = await _httpClient.SendAsync(accountRequest);
        accountResponse.EnsureSuccessStatusCode();
        var accountJson = await accountResponse.Content.ReadAsStringAsync();
        var accountData = JsonSerializer.Deserialize<BinanceAccountResponse>(accountJson);

        // 3. Get current prices
        var priceResponse = await _httpClient.GetAsync("https://api.binance.com/api/v3/ticker/price");
        priceResponse.EnsureSuccessStatusCode();
        var pricesJson = await priceResponse.Content.ReadAsStringAsync();
        var prices = JsonSerializer.Deserialize<List<BinancePriceModel>>(pricesJson);

        var balanceDict = new Dictionary<string, decimal>();

        foreach (var balance in accountData.balances.Where(b => decimal.Parse(b.free, CultureInfo.InvariantCulture) > 0 || decimal.Parse(b.locked, CultureInfo.InvariantCulture) > 0))
        {
            var price = prices.FirstOrDefault(p => p.symbol == balance.asset + "USDT");
            if (price != null)
            {
                var free = decimal.Parse(balance.free, CultureInfo.InvariantCulture);
                var locked = decimal.Parse(balance.locked, CultureInfo.InvariantCulture);
                var totalAsset = free + locked;
                var value = totalAsset * decimal.Parse(price.price, CultureInfo.InvariantCulture);

                portfolio.Balances.Add(new AssetBalanceModel
                {
                    Asset = balance.asset,
                    Free = free,
                    Locked = locked,
                    ValueInUSDT = value
                });

                totalValue += value;
            }
        }

        portfolio.TotalPortfolioValueInUSDT = totalValue;

        // 4. Get Trade History
        var symbols = portfolio.Balances.Select(b => b.Asset + "USDT").ToList();
        var trades = new List<TradeModel>();

        foreach (var symbol in symbols)
        {
            long startTime = GetMillisecondsFromDateTime(new DateTime(2017, 1, 1));
            long endTime = GetMillisecondsFromDateTime(DateTime.UtcNow);

            var tradeQuery = $"symbol={symbol}&startTime={startTime}&limit=1000&timestamp={timestamp}&recvWindow={recvWindow}";
            //var tradeQuery = $"symbol={symbol}&timestamp={timestamp}&recvWindow={recvWindow}";
            var tradeSignature = CreateSignature(tradeQuery);
            var tradeUrl = $"https://api.binance.com/api/v3/myTrades?{tradeQuery}&signature={tradeSignature}";
            var tradeRequest = new HttpRequestMessage(HttpMethod.Get, tradeUrl);
            tradeRequest.Headers.Add("X-MBX-APIKEY", _apiKey);
            var tradeResponse = await _httpClient.SendAsync(tradeRequest);
            var errorContent = await tradeResponse.Content.ReadAsStringAsync();
            if (!tradeResponse.IsSuccessStatusCode) continue;

            var tradeJson = await tradeResponse.Content.ReadAsStringAsync();
            var tradeList = JsonSerializer.Deserialize<List<TradeModel>>(tradeJson);
            if (tradeList != null) trades.AddRange(tradeList);
        }

        portfolio.Trades = trades;

        // 5. Historical PnL (total invested - current value)
        decimal totalInvested = trades.Where(t => t.IsBuyer).Sum(t => t.QuoteQty);
        decimal gainLoss = portfolio.TotalPortfolioValueInUSDT - totalInvested;

        // 6. Order History
        var orders = new List<OrderModel>();
        foreach (var symbol in symbols)
        {
            var orderQuery = $"symbol={symbol}&timestamp={timestamp}&recvWindow={recvWindow}";
            var orderSignature = CreateSignature(orderQuery);
            var orderUrl = $"https://api.binance.com/api/v3/allOrders?{orderQuery}&signature={orderSignature}";
            var orderRequest = new HttpRequestMessage(HttpMethod.Get, orderUrl);
            orderRequest.Headers.Add("X-MBX-APIKEY", _apiKey);
            var orderResponse = await _httpClient.SendAsync(orderRequest);
            if (!orderResponse.IsSuccessStatusCode) continue;

            var orderJson = await orderResponse.Content.ReadAsStringAsync();
            var orderList = JsonSerializer.Deserialize<List<OrderModel>>(orderJson);
            if (orderList != null) orders.AddRange(orderList);
        }

        portfolio.Orders = orders;

        // 7. Investment Summary
        var allocation = new Dictionary<string, decimal>();
        foreach (var b in portfolio.Balances)
        {
            var pct = b.ValueInUSDT / totalValue * 100;
            allocation[b.Asset] = Math.Round(pct, 2);
        }

        var best = portfolio.Balances.OrderByDescending(b => b.ValueInUSDT).FirstOrDefault()?.Asset ?? "N/A";

        portfolio.InvestmentSummary = new InvestmentSummaryModel
        {
            GainLoss = gainLoss,
            BestPerformer = best,
            AllocationPercentage = allocation
        };

        // 8. Portfolio 30-Day Value Chart
        var historyByDate = new Dictionary<string, decimal>();

        foreach (var b in portfolio.Balances)
        {
            var symbol = b.Asset + "USDT";
            try
            {
                var endTime = new DateTimeOffset(DateTime.UtcNow.Date).ToUnixTimeMilliseconds();

                var klineResp = await _httpClient.GetAsync(
                    $"https://api.binance.com/api/v3/klines?symbol={symbol}&interval=1d&limit=30&endTime={endTime}"
                );

                if (!klineResp.IsSuccessStatusCode) continue;

                var klineJson = await klineResp.Content.ReadAsStringAsync();
                var kline = JsonSerializer.Deserialize<List<List<object>>>(klineJson);

                if (kline == null || kline.Count == 0) continue;

                for (int i = 0; i < kline.Count; i++)
                {
                    var candle = kline[i];
                    var close = decimal.Parse(candle[4].ToString(), CultureInfo.InvariantCulture); // close price
                    var ts = long.Parse(candle[0].ToString());
                    var date = DateTimeOffset.FromUnixTimeMilliseconds(ts).UtcDateTime.Date.ToString("yyyy-MM-dd");

                    var dailyValue = close * b.Free;

                    if (historyByDate.ContainsKey(date))
                        historyByDate[date] += dailyValue;
                    else
                        historyByDate[date] = dailyValue;
                }
            }
            catch
            {
                continue;
            }
        }

        portfolio.PortfolioHistory = historyByDate
            .OrderBy(h => h.Key)
            .Select(h => new PortfolioHistoryPoint
            {
                Date = h.Key,
                TotalValueInUSDT = Math.Round(h.Value, 2)
            })
            .ToList();


        if (portfolio.PortfolioHistory.Count >= 2)
        {
            var firstDayValue = portfolio.PortfolioHistory.First().TotalValueInUSDT;
            var lastDayValue = portfolio.PortfolioHistory.Last().TotalValueInUSDT;
            var dollarGain = lastDayValue - firstDayValue;
            var percentGain = firstDayValue == 0 ? 0 : (dollarGain / firstDayValue) * 100;

            portfolio.InvestmentSummary.DollarGain30d = Math.Round(dollarGain, 4);
            portfolio.InvestmentSummary.PercentGain30d = Math.Round(percentGain, 2);
        }

        return portfolio;
    }


    private string CreateSignature(string message)
    {
        var keyBytes = Encoding.UTF8.GetBytes(_apiSecret);
        var messageBytes = Encoding.UTF8.GetBytes(message);
        using var hmac = new HMACSHA256(keyBytes);
        var hash = hmac.ComputeHash(messageBytes);
        return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
    }

    private long GetMillisecondsFromDateTime(DateTime dt)
    {
        return (long)(dt.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
    }
}
