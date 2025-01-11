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

namespace FinTracker.Infrastructure.Services;
public class CoinGeckoApiService : ICoinGeckoApiService
{
    private readonly HttpClient _httpClient;

    public CoinGeckoApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("x-cg-demo-api-key", "CG-Qzs5nxBCRdyc4T5aPLrqGx4N");
    }

    public async Task<NewsArticle> GetCryptoChartData()
    {
        var response = await _httpClient.GetAsync("https://api.coingecko.com/api/v3/coins/bitcoin/market_chart?vs_currency=usd&days=7");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to fetch news from the API");
        }

        var newsApiResponse = await response.Content.ReadAsStringAsync();
        var newsApiData = JsonSerializer.Deserialize<NewsArticle>(newsApiResponse);

        return newsApiData;
    }
}
