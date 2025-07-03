using FinTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using FinTracker.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinTracker.Infrastructure.Persistence;
using System.Net.Http;
using System.Text.Json;
using System.Net;
using FinTracker.Application.Interfaces;
using FinTracker.Application.Models;
using Microsoft.Extensions.Configuration;

namespace FinTracker.Infrastructure.Services;
public class CMCService : ICMCService
{
    private readonly HttpClient _httpClient;

    public CMCService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", config["ApiKey:CoinMarketCapKey"]);
    }

    public async Task<List<CoinPrice>> GetCryptoChartData()
    {
        // 1. Get listings
        var listingResponse = await _httpClient.GetAsync("https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest?limit=10");

        if (!listingResponse.IsSuccessStatusCode)
            throw new Exception("Failed to fetch coin listings");

        var json = await listingResponse.Content.ReadAsStringAsync();
        var doc = JsonDocument.Parse(json);
        var data = doc.RootElement.GetProperty("data");

        var coinList = new List<CoinPrice>();
        var idList = new List<int>();

        foreach (var item in data.EnumerateArray())
        {
            int id = item.GetProperty("id").GetInt32();
            idList.Add(id);

            var usd = item.GetProperty("quote").GetProperty("USD");

            var coin = new CoinPrice
            {
                Id = id,
                Name = item.GetProperty("name").GetString(),
                Symbol = item.GetProperty("symbol").GetString(),
                Price = usd.GetProperty("price").GetDecimal(),
                PercentChange1h = usd.GetProperty("percent_change_1h").GetDecimal(),
                PercentChange24h = usd.GetProperty("percent_change_24h").GetDecimal(),
                PercentChange7d = usd.GetProperty("percent_change_7d").GetDecimal(),
                MarketCap = usd.GetProperty("market_cap").GetDecimal(),
                Volume24h = usd.GetProperty("volume_24h").GetDecimal(),
                CirculatingSupply = item.GetProperty("circulating_supply").GetDecimal()
            };

            coinList.Add(coin);
        }

        // 2. Get logos using the /info endpoint
        string ids = string.Join(",", idList);
        var infoResponse = await _httpClient.GetAsync($"https://pro-api.coinmarketcap.com/v1/cryptocurrency/info?id={ids}");

        if (!infoResponse.IsSuccessStatusCode)
            throw new Exception("Failed to fetch coin logos");

        var infoJson = await infoResponse.Content.ReadAsStringAsync();
        var infoDoc = JsonDocument.Parse(infoJson);
        var infoData = infoDoc.RootElement.GetProperty("data");

        foreach (var coin in coinList)
        {
            if (infoData.TryGetProperty(coin.Id.ToString(), out var coinInfo))
            {
                coin.LogoUrl = coinInfo.GetProperty("logo").GetString();
            }
        }

        return coinList;
    }
}
