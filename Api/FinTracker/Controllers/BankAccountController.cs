using FinTracker.Application.Interfaces;
using FinTracker.Application.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace FinTracker.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BankAccountController : ControllerBase
{
    private readonly IBankAccountService _bankAccountService;

    public BankAccountController(IBankAccountService bankAccountService)
    {
        _bankAccountService = bankAccountService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBankAccounts()
    {
        try
        {
            var result = await _bankAccountService.GetAllAsync();
            if (result.Status != 200) return StatusCode(result.Status, new { message = result.Message });
            return Ok(result.Data);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error", details = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBankAccountById(Guid id)
    {
        try
        {
            var result = await _bankAccountService.GetByIdAsync(id);
            if (result.Status != 200) return StatusCode(result.Status, new { message = result.Message });
            return Ok(result.Data);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error", details = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateBankAccount([FromBody] CreateBankAccountRequest request)
    {
        try
        {
            var result = await _bankAccountService.CreateAsync(request);
            if (result.Status != 200) return StatusCode(result.Status, new { message = result.Message });
            return CreatedAtAction(nameof(GetBankAccountById), new { id = result.Data.Id }, result.Data);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error", details = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBankAccount(Guid id, [FromBody] UpdateBankAccountRequest request)
    {
        try
        {
            var result = await _bankAccountService.UpdateAsync(id, request);
            if (result.Status != 200) return StatusCode(result.Status, new { message = result.Message });
            return Ok(result.Data);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error", details = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBankAccount(Guid id)
    {
        try
        {
            var result = await _bankAccountService.DeleteAsync(id);
            if (result.Status != 200) return StatusCode(result.Status, new { message = result.Message });
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error", details = ex.Message });
        }
    }
}
