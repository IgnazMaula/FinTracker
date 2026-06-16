using FinTracker.Application.Interfaces;
using FinTracker.Utilities.Helper;
using Microsoft.AspNetCore.Mvc;

namespace FinTracker.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BankTransactionController : ControllerBase
{
    private readonly IBankTransactionService _bankTransactionService;
    private readonly ITransactionCSVService _transactionCsvService;

    public BankTransactionController(IBankTransactionService bankTransactionService, ITransactionCSVService transactionCSVService)
    {
        _bankTransactionService = bankTransactionService;
        _transactionCsvService = transactionCSVService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBankTransactions()
    {
        try
        {
            var result = await _bankTransactionService.GetAllAsync();
            if (result.Status != 200) return StatusCode(result.Status, new { message = result.Message });
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
            var result = await _bankTransactionService.GetByIdAsync(id);
            if (result.Status != 200) return StatusCode(result.Status, new { message = result.Message });
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
            var result = await _bankTransactionService.GetByBankIdAsync(id);
            if (result.Status != 200) return StatusCode(result.Status, new { message = result.Message });
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
            var result = await _bankTransactionService.GetMonthlyByUserIdAsync(id);
            if (result.Status != 200) return StatusCode(result.Status, new { message = result.Message });
            return Ok(result.Data.MonthlyBankAccountTransaction);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error", details = ex.Message });
        }
    }

    [HttpPost("Upload/{id}")]
    public async Task<IActionResult> UploadCsv(Guid id, [FromForm] IFormFile? file)
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

            await _transactionCsvService.ProcessCsvAsync(file.OpenReadStream(), year, id);
            return Ok(new { message = "File uploaded and processed successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = $"Error processing file: {ex.Message}" });
        }
    }
}
