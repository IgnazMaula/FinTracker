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
public class AlphavantageNewsService : IAlphavantageNewsService
{
    private readonly HttpClient _httpClient;

    public AlphavantageNewsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<NewsArticle> GetNewsFeed()
    {
        var response = await _httpClient.GetAsync("https://www.alphavantage.co/query?function=NEWS_SENTIMENT&tickers=COIN,CRYPTO:BTC,FOREX:USD&time_from=20220410T0130&limit=1000&apikey=demo");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to fetch news from the API");
        }

        var newsApiResponse = await response.Content.ReadAsStringAsync();
        var newsApiData = JsonSerializer.Deserialize<NewsArticle>(newsApiResponse);

        return newsApiData;
    }
}
