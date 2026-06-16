using FinTracker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinTracker.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CashflowController : ControllerBase
{
    private readonly ICashflowService _cashflowService;

    public CashflowController(ICashflowService cashflowService)
    {
        _cashflowService = cashflowService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCashflowDashboard(Guid userId)
    {
        try
        {
            var result = await _cashflowService.GetDashboardAsync(userId);
            if (result.Status != 200) return StatusCode(result.Status, new { message = result.Message });
            return Ok(result.Data);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error", details = ex.Message });
        }
    }
}
