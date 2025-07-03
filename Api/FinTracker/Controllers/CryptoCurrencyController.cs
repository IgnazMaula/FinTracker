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

    public CryptoCurrencyController(ICMCService cmcService)
    {
        _cmcService = cmcService;
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
}