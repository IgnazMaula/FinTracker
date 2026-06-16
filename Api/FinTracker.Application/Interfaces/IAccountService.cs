using FinTracker.Application.Common;
using FinTracker.Application.Models.DTOs;
using FinTracker.Application.Models.Requests;

namespace FinTracker.Application.Interfaces;

public interface IAccountService
{
    Task<BaseResponse<List<AccountDTO>>> GetAllAsync();
    Task<BaseResponse<AccountDTO>> GetByIdAsync(Guid id);
    Task<BaseResponse<AccountDTO>> CreateAsync(CreateAccountRequest request);
    Task<BaseResponse<AccountDTO>> UpdateAsync(Guid id, UpdateAccountRequest request);
    Task<BaseResponse<bool>> DeleteAsync(Guid id);
}
