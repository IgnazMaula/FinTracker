using AutoMapper;
using FinTracker.Application.Common;
using FinTracker.Application.Interfaces;
using FinTracker.Application.Models.DTOs;
using FinTracker.Domain.Entities;
using FinTracker.Domain.Interfaces;

namespace FinTracker.Application.Services;

public class BankTransactionService : IBankTransactionService
{
    private readonly IRepository<BankTransaction> _bankTransactionRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IBankTransactionRepository _specializedBankTransactionRepository;
    private readonly IMapper _mapper;

    public BankTransactionService(
        IRepository<BankTransaction> bankTransactionRepository,
        IRepository<User> userRepository,
        IBankAccountRepository bankAccountRepository,
        IBankTransactionRepository specializedBankTransactionRepository,
        IMapper mapper)
    {
        _bankTransactionRepository = bankTransactionRepository;
        _userRepository = userRepository;
        _bankAccountRepository = bankAccountRepository;
        _specializedBankTransactionRepository = specializedBankTransactionRepository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<List<BankTransactionDTO>>> GetAllAsync()
    {
        var response = new BaseResponse<List<BankTransactionDTO>>();

        try
        {
            var transactions = await _bankTransactionRepository.GetAllAsync();
            response.Data = _mapper.Map<List<BankTransactionDTO>>(transactions);
            response.SetReturnSuccessStatus();
        }
        catch (Exception ex)
        {
            response.SetReturnErrorStatus($"Error: {ex.Message}");
        }

        return response;
    }

    public async Task<BaseResponse<BankTransactionDTO>> GetByIdAsync(Guid id)
    {
        var response = new BaseResponse<BankTransactionDTO>();

        try
        {
            var transaction = await _bankTransactionRepository.GetByIdAsync(id);
            if (transaction == null)
            {
                response.SetReturnErrorStatus("Result not found");
                return response;
            }

            response.Data = _mapper.Map<BankTransactionDTO>(transaction);
            response.SetReturnSuccessStatus();
        }
        catch (Exception ex)
        {
            response.SetReturnErrorStatus($"Error: {ex.Message}");
        }

        return response;
    }

    public async Task<BaseResponse<List<BankTransactionDTO>>> GetByBankIdAsync(Guid bankId)
    {
        var response = new BaseResponse<List<BankTransactionDTO>>();

        try
        {
            var transactions = await _specializedBankTransactionRepository.GetBankTransactionByBankIdAsync(bankId);
            if (transactions == null)
            {
                response.SetReturnErrorStatus("Result not found");
                return response;
            }

            response.Data = _mapper.Map<List<BankTransactionDTO>>(transactions);
            response.SetReturnSuccessStatus();
        }
        catch (Exception ex)
        {
            response.SetReturnErrorStatus($"Error: {ex.Message}");
        }

        return response;
    }

    public async Task<BaseResponse<MonthlyBankTransactionDTO>> GetMonthlyByUserIdAsync(Guid userId)
    {
        var response = new BaseResponse<MonthlyBankTransactionDTO>();

        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                response.SetReturnErrorStatus("User not found");
                return response;
            }

            var bankAccounts = await _bankAccountRepository.GetBankAccountByUserIdAsync(user.Id);
            if (bankAccounts.Count == 0)
            {
                response.SetReturnErrorStatus("Bank account not found");
                return response;
            }

            var bankTransactions = await _specializedBankTransactionRepository.GetBankTransactionByUserIdAsync(user.Id);
            var groupedTransactions = bankTransactions
                .GroupBy(t => new { t.BankAccountId, t.TransactionDate.Year, t.TransactionDate.Month })
                .Select(g => new MonthlyBankAccountTransaction
                {
                    BankAccountId = g.Key.BankAccountId,
                    Period = $"{g.Key.Year % 100:D2}/{g.Key.Month:D2}",
                    TotalCredit = g.Where(t => t.TransactionType == "Credit").Sum(t => t.TransactionAmount) ?? 0,
                    TotalDebit = g.Where(t => t.TransactionType == "Debit").Sum(t => Math.Abs(t.TransactionAmount ?? 0))
                })
                .Select(t => new MonthlyBankAccountTransaction
                {
                    BankAccountId = t.BankAccountId,
                    Period = t.Period,
                    TotalCredit = t.TotalCredit,
                    TotalDebit = t.TotalDebit,
                    SurplusDeficit = t.TotalCredit - t.TotalDebit
                })
                .OrderByDescending(g => g.Period)
                .Reverse()
                .Take(12)
                .ToList();

            response.Data = new MonthlyBankTransactionDTO
            {
                MonthlyBankAccountTransaction = groupedTransactions
            };
            response.SetReturnSuccessStatus();
        }
        catch (Exception ex)
        {
            response.SetReturnErrorStatus($"Error: {ex.Message}");
        }

        return response;
    }
}
