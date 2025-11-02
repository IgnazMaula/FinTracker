using Microsoft.AspNetCore.Mvc;
using FinTracker.Domain.Interfaces;
using MediatR;
using FinTracker.Application.Features.Dashboard.Query;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using FinTracker.Application.Models.DTOs;
using FinTracker.Domain.Entities;

namespace FinTracker.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CashflowController : BaseApiController
{

    [HttpGet]
    public async Task<IActionResult> GetCashflowDashboard(Guid userId)
    {
        try
        {
            var result = await Mediator.Send(new GetCashflowDashboardQuery(userId));

            if (result.Status != 200)
                return StatusCode(result.Status, new { message = result.Message });

            return Ok(result.Data);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error", details = ex.Message });
        }
    }
}