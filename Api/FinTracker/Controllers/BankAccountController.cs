using Microsoft.AspNetCore.Mvc;
using FinTracker.Domain.Interfaces;
using MediatR;
using FinTracker.Application.Features.BankAccounts.Query;
using FinTracker.Application.Features.BankAccounts.Command;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using FinTracker.Application.Models.DTOs;
using FinTracker.Domain.Entities;

namespace FinTracker.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BankAccountController : BaseApiController
{

    [HttpGet]
    public async Task<IActionResult> GetAllBankAccounts()
    {
        try
        {
            //var result = await Mediator.Send(new GetAllBankAccountsQuery());

            //if (result.Status != 200)
            //    return StatusCode(result.Status, new { message = result.Message });

            //return Ok(result.Data);

            var bankAccounts = new List<BankAccount>()
            {
                new BankAccount
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    Name = "Main Saving Account",
                    BankName = "BCA",
                    AccountNo = "69696969",
                    Currency = "IDR",
                    CurrentBalance = 124000000,
                    InitialBalance = 124000000
                }
            };

            return Ok(bankAccounts);
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
            //var query = new GetBankAccountByIdQuery(id);
            //var result = await Mediator.Send(query);

            //if (result.Status != 200)
            //    return StatusCode(result.Status, new { message = result.Message });

            //return Ok(result.Data);

            var bankAccount = new BankAccount
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Name = "Main Saving Account",
                BankName = "BCA",
                AccountNo = "69696969",
                Currency = "IDR",
                CurrentBalance = 124000000,
                InitialBalance = 124000000
            };

            return Ok(bankAccount);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error", details = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateBankAccount([FromBody] CreateBankAccountCommand command)
    {
        try
        {
            var result = await Mediator.Send(command);

            if (result.Status != 200)
                return StatusCode(result.Status, new { message = result.Message });

            return CreatedAtAction(nameof(GetBankAccountById), new { id = result.Data.Id }, result.Data);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error", details = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBankAccount(Guid id, [FromBody] UpdateBankAccountCommand command)
    {
        try
        {
            command.Id = id;
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBankAccount(Guid id)
    {
        try
        {
            var command = new DeleteBankAccountCommand(id);
            var result = await Mediator.Send(command);

            if (result.Status != 200)
                return StatusCode(result.Status, new { message = result.Message });

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error", details = ex.Message });
        }
    }
}