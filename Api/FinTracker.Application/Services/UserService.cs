using AutoMapper;
using FinTracker.Application.Common;
using FinTracker.Application.Interfaces;
using FinTracker.Application.Models.DTOs;
using FinTracker.Application.Models.Requests;
using FinTracker.Domain.Entities;
using FinTracker.Domain.Interfaces;

namespace FinTracker.Application.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _userRepository;
    private readonly IMapper _mapper;

    public UserService(IRepository<User> userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<List<UserDTO>>> GetAllAsync()
    {
        var response = new BaseResponse<List<UserDTO>>();

        try
        {
            var users = await _userRepository.GetAllAsync();
            response.Data = _mapper.Map<List<UserDTO>>(users);
            response.SetReturnSuccessStatus();
        }
        catch (Exception ex)
        {
            response.SetReturnErrorStatus($"Error: {ex.Message}");
        }

        return response;
    }

    public async Task<BaseResponse<UserDTO>> GetByIdAsync(Guid id)
    {
        var response = new BaseResponse<UserDTO>();

        try
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                response.SetReturnErrorStatus("User not found");
                return response;
            }

            response.Data = _mapper.Map<UserDTO>(user);
            response.SetReturnSuccessStatus();
        }
        catch (Exception ex)
        {
            response.SetReturnErrorStatus($"Error: {ex.Message}");
        }

        return response;
    }

    public async Task<BaseResponse<UserDTO>> CreateAsync(CreateUserRequest request)
    {
        var response = new BaseResponse<UserDTO>();

        try
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Email = request.Email,
                PasswordHash = request.PasswordHash,
                Role = request.Role
            };

            await _userRepository.CreateAsync(user);

            response.Data = _mapper.Map<UserDTO>(user);
            response.SetReturnSuccessStatus();
        }
        catch (Exception ex)
        {
            response.SetReturnErrorStatus($"Error: {ex.Message}");
        }

        return response;
    }

    public async Task<BaseResponse<UserDTO>> UpdateAsync(Guid id, UpdateUserRequest request)
    {
        var response = new BaseResponse<UserDTO>();

        try
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                response.SetReturnErrorStatus("User not found");
                return response;
            }

            user.Username = request.Username;
            user.Email = request.Email;
            user.PasswordHash = request.PasswordHash;
            user.Role = request.Role;

            await _userRepository.UpdateAsync(user);

            response.Data = _mapper.Map<UserDTO>(user);
            response.SetReturnSuccessStatus();
        }
        catch (Exception ex)
        {
            response.SetReturnErrorStatus($"Error: {ex.Message}");
        }

        return response;
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                response.Data = false;
                response.SetReturnErrorStatus("User not found");
                return response;
            }

            await _userRepository.DeleteAsync(id);
            response.Data = true;
            response.SetReturnSuccessStatus();
        }
        catch (Exception ex)
        {
            response.Data = false;
            response.SetReturnErrorStatus($"Error: {ex.Message}");
        }

        return response;
    }
}
