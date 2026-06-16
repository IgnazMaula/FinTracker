using FinTracker.Application.Interfaces;
using FinTracker.Application.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace FinTracker.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DCACalculatorController : ControllerBase
{
    private readonly IDCACalculatorService _dcaCalculatorService;

    public DCACalculatorController(IDCACalculatorService dcaCalculatorService)
    {
        _dcaCalculatorService = dcaCalculatorService;
    }

    [HttpPost]
    public async Task<IActionResult> SubmitDCACalculator([FromBody] DCACalculatorRequest request)
    {
        try
        {
            var result = await _dcaCalculatorService.SubmitAsync(request);
            if (result.Status != 200) return StatusCode(result.Status, new { message = result.Message });
            return Ok(result.Data);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error", details = ex.Message });
        }
    }
}
