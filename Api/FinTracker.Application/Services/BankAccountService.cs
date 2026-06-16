using AutoMapper;
using FinTracker.Application.Common;
using FinTracker.Application.Interfaces;
using FinTracker.Application.Models.DTOs;
using FinTracker.Application.Models.Requests;
using FinTracker.Domain.Entities;
using FinTracker.Domain.Interfaces;

namespace FinTracker.Application.Services;

public class BankAccountService : IBankAccountService
{
    private readonly IRepository<BankAccount> _bankAccountRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IMapper _mapper;

    public BankAccountService(IRepository<BankAccount> bankAccountRepository, IRepository<User> userRepository, IMapper mapper)
    {
        _bankAccountRepository = bankAccountRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<List<BankAccountDTO>>> GetAllAsync()
    {
        var response = new BaseResponse<List<BankAccountDTO>>();

        try
        {
            var accounts = await _bankAccountRepository.GetAllAsync();
            response.Data = _mapper.Map<List<BankAccountDTO>>(accounts);
            response.SetReturnSuccessStatus();
        }
        catch (Exception ex)
        {
            response.SetReturnErrorStatus($"Error: {ex.Message}");
        }

        return response;
    }

    public async Task<BaseResponse<BankAccountDTO>> GetByIdAsync(Guid id)
    {
        var response = new BaseResponse<BankAccountDTO>();

        try
        {
            var account = await _bankAccountRepository.GetByIdAsync(id);
            if (account == null)
            {
                response.SetReturnErrorStatus("Account not found");
                return response;
            }

            response.Data = _mapper.Map<BankAccountDTO>(account);
            response.SetReturnSuccessStatus();
        }
        catch (Exception ex)
        {
            response.SetReturnErrorStatus($"Error: {ex.Message}");
        }

        return response;
    }

    public async Task<BaseResponse<BankAccountDTO>> CreateAsync(CreateBankAccountRequest request)
    {
        var response = new BaseResponse<BankAccountDTO>();

        try
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            var account = new BankAccount
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                User = user,
                Name = request.Name,
                BankName = request.BankName,
                AccountNo = request.AccountNo,
                Currency = request.Currency,
                InitialBalance = request.InitialBalance,
                CurrentBalance = request.InitialBalance
            };

            await _bankAccountRepository.CreateAsync(account);

            response.Data = _mapper.Map<BankAccountDTO>(account);
            response.SetReturnSuccessStatus();
        }
        catch (Exception ex)
        {
            response.SetReturnErrorStatus($"Error: {ex.Message}");
        }

        return response;
    }

    public async Task<BaseResponse<BankAccountDTO>> UpdateAsync(Guid id, UpdateBankAccountRequest request)
    {
        var response = new BaseResponse<BankAccountDTO>();

        try
        {
            var account = await _bankAccountRepository.GetByIdAsync(id);
            if (account == null)
            {
                response.SetReturnErrorStatus("Account not found");
                return response;
            }

            account.Name = request.Name;
            account.AccountNo = request.AccountNo;
            account.BankName = request.BankName;

            await _bankAccountRepository.UpdateAsync(account);

            response.Data = _mapper.Map<BankAccountDTO>(account);
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
            var account = await _bankAccountRepository.GetByIdAsync(id);
            if (account == null)
            {
                response.Data = false;
                response.SetReturnErrorStatus("Account not found");
                return response;
            }

            await _bankAccountRepository.DeleteAsync(id);
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
