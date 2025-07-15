using Microsoft.AspNetCore.Mvc;
using FinTracker.Domain.Interfaces;
using MediatR;
using FinTracker.Application.Services;
using FinTracker.Domain.Entities;
using FinTracker.Application.Interfaces;

namespace FinTracker.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CryptoCurrencyController : ControllerBase
{
    private readonly ICMCService _cmcService;
    private readonly IBinancePortfolioService _binancePortfolioService;

    public CryptoCurrencyController(ICMCService cmcService, IBinancePortfolioService binancePortfolioService)
    {
        _cmcService = cmcService;
        _binancePortfolioService = binancePortfolioService;

    }

    [HttpGet]
    public async Task<IActionResult> GetCryptoCurrencyFeedAsync()
    {
        try
        {
            var result = await _cmcService.GetCryptoChartData();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("BinancePortfolio")]
    public async Task<IActionResult> GetBinancePortfolio()
    {
        try
        {
            var result = await _binancePortfolioService.GetPortfolioAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}