using FinTracker.Application.Common;
using FinTracker.Application.Models.DTOs;
using FinTracker.Application.Models.Requests;

namespace FinTracker.Application.Interfaces;

public interface IUserService
{
    Task<BaseResponse<List<UserDTO>>> GetAllAsync();
    Task<BaseResponse<UserDTO>> GetByIdAsync(Guid id);
    Task<BaseResponse<UserDTO>> CreateAsync(CreateUserRequest request);
    Task<BaseResponse<UserDTO>> UpdateAsync(Guid id, UpdateUserRequest request);
    Task<BaseResponse<bool>> DeleteAsync(Guid id);
}
