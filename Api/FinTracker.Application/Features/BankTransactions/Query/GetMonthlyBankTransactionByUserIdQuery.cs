using AutoMapper;
using FinTracker.Application.Common;
using FinTracker.Domain.Entities;
using MediatR;
using FinTracker.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using FinTracker.Application.Models.DTOs;

namespace FinTracker.Application.Features.BankTransactions.Query;

public class GetMonthlyBankTransactionByUserIdQuery : IRequest<BaseResponse<MonthlyBankTransactionDTO>>
{
    public Guid Id { get; set; }

    public GetMonthlyBankTransactionByUserIdQuery(Guid id)
    {
        Id = id;
    }
}

public class GetMonthlyBankTransactionByUserIdHandler : IRequestHandler<GetMonthlyBankTransactionByUserIdQuery, BaseResponse<MonthlyBankTransactionDTO>>
{
    private readonly IRepository<User> _userRepository;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IBankTransactionRepository _bankTransactionRepository;
    private readonly IMapper _mapper;

    public GetMonthlyBankTransactionByUserIdHandler(IRepository<User> userRepository, IBankAccountRepository bankAccountRepository, IBankTransactionRepository bankTransactionRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _bankAccountRepository = bankAccountRepository;
        _bankTransactionRepository = bankTransactionRepository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<MonthlyBankTransactionDTO>> Handle(GetMonthlyBankTransactionByUserIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<MonthlyBankTransactionDTO>();

        try
        {
            var result = new MonthlyBankTransactionDTO();

            var user = await _userRepository.GetByIdAsync(request.Id);
            if (user == null)
            {
                response.SetReturnErrorStatus("User not found");
            }
            var bankAccounts = await _bankAccountRepository.GetBankAccountByUserIdAsync(user.Id);
            if (bankAccounts.Count == 0)
            {
                response.SetReturnErrorStatus("Bank account not found");
            }

            var bankTransactions = await _bankTransactionRepository.GetBankTransactionByUserIdAsync(user.Id);

            var groupedTransactions = bankTransactions
            .GroupBy(t => new { t.BankAccountId, t.TransactionDate.Year, t.TransactionDate.Month }) // Group by Bank ID + Year + Month
            .Select(g => new MonthlyBankAccountTransaction
            {
                BankAccountId = g.Key.BankAccountId,
                Period = $"{g.Key.Year:D4}/{g.Key.Month:D2}",
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
            .ToList();

            result.MonthlyBankAccountTransaction = groupedTransactions;


            if (result == null)
            {
                response.SetReturnErrorStatus("Result not found");
            }
            else
            {
                response.Data = _mapper.Map<MonthlyBankTransactionDTO>(result);
                response.SetReturnSuccessStatus();
            }
        }
        catch (Exception ex)
        {
            response.SetReturnErrorStatus($"Error: {ex.Message}");
        }

        return response;
    }
}
