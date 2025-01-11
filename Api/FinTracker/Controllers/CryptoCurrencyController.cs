using Microsoft.AspNetCore.Mvc;
using FinTracker.Domain.Interfaces;
using MediatR;
using FinTracker.Application.Services;
using FinTracker.Domain.Entities;

namespace FinTracker.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CryptoCurrencyController : ControllerBase
{
    private readonly ICoinGeckoApiService _coinGeckoApiService;

    public CryptoCurrencyController(ICoinGeckoApiService coinGeckoApiService)
    {
        _coinGeckoApiService = coinGeckoApiService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCryptoCurrencyFeedAsync()
    {
        try
        {
            // Call the CryptoCurrencyFeedService to fetch the news feed
            var newsFeed = await _coinGeckoApiService.GetCryptoChartData();

            // Return the news feed as a JSON response
            return Ok(newsFeed);
        }
        catch (Exception ex)
        {
            // Handle any errors (optional)
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}