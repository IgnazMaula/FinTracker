using Microsoft.AspNetCore.Mvc;
using FinTracker.Domain.Interfaces;
using MediatR;
using FinTracker.Application.Features.DCACalculators.Command;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using FinTracker.Application.Models.DTOs;
using FinTracker.Domain.Entities;
using FinTracker.Application.Features.DCACalculators.Command;

namespace FinTracker.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DCACalculatorController : BaseApiController
{

    [HttpPost]
    public async Task<IActionResult> SubmitDCACalculator([FromBody] SubmitDCACalculatorCommand command)
    {
        try
        {
            var result = await Mediator.Send(command);

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