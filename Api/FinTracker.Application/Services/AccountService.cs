using AutoMapper;
using FinTracker.Application.Common;
using FinTracker.Application.Interfaces;
using FinTracker.Application.Models.DTOs;
using FinTracker.Application.Models.Requests;
using FinTracker.Domain.Entities;
using FinTracker.Domain.Interfaces;

namespace FinTracker.Application.Services;

public class AccountService : IAccountService
{
    private readonly IRepository<Account> _accountRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IMapper _mapper;

    public AccountService(IRepository<Account> accountRepository, IRepository<User> userRepository, IMapper mapper)
    {
        _accountRepository = accountRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<List<AccountDTO>>> GetAllAsync()
    {
        var response = new BaseResponse<List<AccountDTO>>();

        try
        {
            var accounts = await _accountRepository.GetAllAsync();
            response.Data = _mapper.Map<List<AccountDTO>>(accounts);
            response.SetReturnSuccessStatus();
        }
        catch (Exception ex)
        {
            response.SetReturnErrorStatus($"Error: {ex.Message}");
        }

        return response;
    }

    public async Task<BaseResponse<AccountDTO>> GetByIdAsync(Guid id)
    {
        var response = new BaseResponse<AccountDTO>();

        try
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if (account == null)
            {
                response.SetReturnErrorStatus("Account not found");
                return response;
            }

            response.Data = _mapper.Map<AccountDTO>(account);
            response.SetReturnSuccessStatus();
        }
        catch (Exception ex)
        {
            response.SetReturnErrorStatus($"Error: {ex.Message}");
        }

        return response;
    }

    public async Task<BaseResponse<AccountDTO>> CreateAsync(CreateAccountRequest request)
    {
        var response = new BaseResponse<AccountDTO>();

        try
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            var account = new Account
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                User = user,
                Name = request.Name,
                Type = request.Type,
                Institution = request.Institution,
                Description = request.Description,
                InitialBalance = request.InitialBalance,
                CurrentBalance = request.CurrentBalance
            };

            await _accountRepository.CreateAsync(account);

            response.Data = _mapper.Map<AccountDTO>(account);
            response.SetReturnSuccessStatus();
        }
        catch (Exception ex)
        {
            response.SetReturnErrorStatus($"Error: {ex.Message}");
        }

        return response;
    }

    public async Task<BaseResponse<AccountDTO>> UpdateAsync(Guid id, UpdateAccountRequest request)
    {
        var response = new BaseResponse<AccountDTO>();

        try
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if (account == null)
            {
                response.SetReturnErrorStatus("Account not found");
                return response;
            }

            account.Name = request.Name;
            account.Type = request.Type;
            account.Institution = request.Institution;
            account.Description = request.Description;

            await _accountRepository.UpdateAsync(account);

            response.Data = _mapper.Map<AccountDTO>(account);
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
            var account = await _accountRepository.GetByIdAsync(id);
            if (account == null)
            {
                response.Data = false;
                response.SetReturnErrorStatus("Account not found");
                return response;
            }

            await _accountRepository.DeleteAsync(id);
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
