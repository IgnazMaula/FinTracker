using Microsoft.AspNetCore.Mvc;
using FinTracker.Domain.Interfaces;
using MediatR;
using FinTracker.Application.Features.Accounts.Query;
using FinTracker.Application.Features.Accounts.Command;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace FinTracker.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : BaseApiController
{

    [HttpGet]
    public async Task<IActionResult> GetAllAccounts()
    {
        var result = await Mediator.Send(new GetAllAccountsQuery());
        return Ok(result.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAccountById(Guid id)
    {
        var query = new GetAccountByIdQuery(id);
        var result = await Mediator.Send(query);
        if (result == null) return NotFound();
        return Ok(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountCommand command)
    {
        var result = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetAccountById), new { id = result.Data.Id }, result.Data);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAccount(Guid id, [FromBody] UpdateAccountCommand command)
    {
        command.Id = id;
        var result = await Mediator.Send(command);
        return Ok(result.Data);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccount(Guid id)
    {
        var command = new DeleteAccountCommand(id);
        await Mediator.Send(command);
        return NoContent();
    }
}