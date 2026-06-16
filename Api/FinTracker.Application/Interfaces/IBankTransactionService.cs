using FinTracker.Application.Common;
using FinTracker.Application.Models.DTOs;

namespace FinTracker.Application.Interfaces;

public interface IBankTransactionService
{
    Task<BaseResponse<List<BankTransactionDTO>>> GetAllAsync();
    Task<BaseResponse<BankTransactionDTO>> GetByIdAsync(Guid id);
    Task<BaseResponse<List<BankTransactionDTO>>> GetByBankIdAsync(Guid bankId);
    Task<BaseResponse<MonthlyBankTransactionDTO>> GetMonthlyByUserIdAsync(Guid userId);
}
