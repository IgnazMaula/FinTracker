using Microsoft.AspNetCore.Mvc;
using FinTracker.Domain.Interfaces;
using MediatR;
using FinTracker.Application.Features.BankTransactions.Query;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using FinTracker.Domain.Entities;
using FinTracker.Application.Interfaces;
using FinTracker.Common.Shared.Model;
using FinTracker.Utilities.Helper;

namespace FinTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankTransactionController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ITransactionCSVService _transactionCsvService;

        public BankTransactionController(IMediator mediator, ITransactionCSVService transactionCSVService)
        {
            _mediator = mediator;
            _transactionCsvService = transactionCSVService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBankTransactions()
        {
            try
            {
                var query = new GetAllBankTransactionsQuery();
                var result = await _mediator.Send(query);

                if (result.Status != 200)
                    return StatusCode(result.Status, new { message = result.Message });

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBankTransactionById(Guid id)
        {
            try
            {
                var query = new GetBankTransactionByIdQuery(id);
                var result = await _mediator.Send(query);

                if (result.Status != 200)
                    return StatusCode(result.Status, new { message = result.Message });

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        [HttpGet("ByBankId/{id}")]
        public async Task<IActionResult> GetBankTransactionByBankId(Guid id)
        {
            try
            {
                var query = new GetBankTransactionByBankIdQuery(id);
                var result = await _mediator.Send(query);

                if (result.Status != 200)
                    return StatusCode(result.Status, new { message = result.Message });

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        [HttpGet("GetMonthlyBankTransactionByUserId/{id}")]
        public async Task<IActionResult> GetMonthlyBankTransactionByUserId(Guid id)
        {
            try
            {
                var query = new GetMonthlyBankTransactionByUserIdQuery(id);
                var result = await _mediator.Send(query);

                if (result.Status != 200)
                    return StatusCode(result.Status, new { message = result.Message });

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        [HttpPost("Upload/{id}")]
        public async Task<IActionResult> UploadCsv(Guid Id, [FromForm] IFormFile? file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "No file uploaded." });
            }

            try
            {

                var (isValid, year) = YearMonthFormatValidator.ValidateAndExtractYear(file.FileName);

                if (!isValid)
                {
                    return BadRequest(new { message = "Filename must be in 'YYYYMM' format." });
                }

                await _transactionCsvService.ProcessCsvAsync(file.OpenReadStream(), year, Id);

                return Ok(new { message = "File uploaded and processed successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error processing file: {ex.Message}" });
            }
        }


    }
}
