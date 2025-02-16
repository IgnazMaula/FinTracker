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
using FinTracker.Application.Models.DTOs;
using Newtonsoft.Json;

namespace FinTracker.Infrastructure.Services;
public class TipRankspiService : ITipRanksApiService
{
    private readonly HttpClient _httpClient;

    public TipRankspiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("x-cg-demo-api-key", "CG-Qzs5nxBCRdyc4T5aPLrqGx4N");
    }

    public async Task<List<DCAHistoryDataDTO>> GetHistoricalPrice(string ticker)
    {       

        var response = await _httpClient.GetAsync($"https://www.tipranks.com/api/stocks/getHistoricalPriceExtended?daysBack=3650&name={ticker}");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to data from the API");
        }

        var apiResponse = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<List<DCAHistoryDataDTO>>(apiResponse);


        return result;
    }
}
