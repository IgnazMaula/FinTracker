using AutoMapper;
using FinTracker.Application.Common;
using FinTracker.Application.Models.DTOs;
using FinTracker.Domain.Entities;
using MediatR;
using FinTracker.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Application.Features.BankTransactions.Query;

public class GetAllBankTransactionsQuery : IRequest<BaseResponse<List<BankTransactionDTO>>>
{
}

public class GetAllTransactionsHandler : IRequestHandler<GetAllBankTransactionsQuery, BaseResponse<List<BankTransactionDTO>>>
{
    private readonly IRepository<BankTransaction> _repository;
    private readonly IMapper _mapper;

    public GetAllTransactionsHandler(IRepository<BankTransaction> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<List<BankTransactionDTO>>> Handle(GetAllBankTransactionsQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<List<BankTransactionDTO>>();

        try
        {
            var result = await _repository.GetAllAsync();
            var data = _mapper.Map<List<BankTransactionDTO>>(result);

            response.Data = data;
            response.SetReturnSuccessStatus();
        }
        catch (Exception ex)
        {
            response.SetReturnErrorStatus($"Error: {ex.Message}");
        }

        return response;
    }
}
