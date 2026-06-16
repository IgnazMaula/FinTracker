using FinTracker.Application.Interfaces;
using FinTracker.Application.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace FinTracker.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAccounts()
    {
        var result = await _accountService.GetAllAsync();
        return Ok(result.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAccountById(Guid id)
    {
        var result = await _accountService.GetByIdAsync(id);
        if (result.Status != 200) return StatusCode(result.Status, new { message = result.Message });
        return Ok(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest request)
    {
        var result = await _accountService.CreateAsync(request);
        if (result.Status != 200) return StatusCode(result.Status, new { message = result.Message });
        return CreatedAtAction(nameof(GetAccountById), new { id = result.Data.Id }, result.Data);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAccount(Guid id, [FromBody] UpdateAccountRequest request)
    {
        var result = await _accountService.UpdateAsync(id, request);
        if (result.Status != 200) return StatusCode(result.Status, new { message = result.Message });
        return Ok(result.Data);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccount(Guid id)
    {
        var result = await _accountService.DeleteAsync(id);
        if (result.Status != 200) return StatusCode(result.Status, new { message = result.Message });
        return NoContent();
    }
}
