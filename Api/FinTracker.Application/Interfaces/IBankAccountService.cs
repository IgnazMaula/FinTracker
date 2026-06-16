using FinTracker.Application.Common;
using FinTracker.Application.Models.DTOs;
using FinTracker.Application.Models.Requests;

namespace FinTracker.Application.Interfaces;

public interface IBankAccountService
{
    Task<BaseResponse<List<BankAccountDTO>>> GetAllAsync();
    Task<BaseResponse<BankAccountDTO>> GetByIdAsync(Guid id);
    Task<BaseResponse<BankAccountDTO>> CreateAsync(CreateBankAccountRequest request);
    Task<BaseResponse<BankAccountDTO>> UpdateAsync(Guid id, UpdateBankAccountRequest request);
    Task<BaseResponse<bool>> DeleteAsync(Guid id);
}
