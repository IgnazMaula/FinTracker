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

public class GetBankTransactionByBankIdQuery : IRequest<BaseResponse<List<BankTransactionDTO>>>
{
    public Guid Id { get; set; }

    public GetBankTransactionByBankIdQuery(Guid id)
    {
        Id = id;
    }
}

public class GetBankTransactionByBankIdHandler : IRequestHandler<GetBankTransactionByBankIdQuery, BaseResponse<List<BankTransactionDTO>>>
{
    private readonly IBankTransactionRepository _repository;
    private readonly IMapper _mapper;

    public GetBankTransactionByBankIdHandler(IBankTransactionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<List<BankTransactionDTO>>> Handle(GetBankTransactionByBankIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<List<BankTransactionDTO>>();

        try
        {
            var result = await _repository.GetBankTransactionByBankIdAsync(request.Id);

            if (result == null)
            {
                response.SetReturnErrorStatus("Result not found");
            }
            else
            {
                response.Data = _mapper.Map<List<BankTransactionDTO>>(result);
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
