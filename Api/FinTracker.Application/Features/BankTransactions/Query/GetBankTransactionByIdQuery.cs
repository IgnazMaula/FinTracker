﻿using AutoMapper;
using FinTracker.Application.Common;
using FinTracker.Domain.Entities;
using MediatR;
using FinTracker.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using FinTracker.Application.Models.DTOs;

namespace FinTracker.Application.Features.BankTransactions.Query;

public class GetBankTransactionByIdQuery : IRequest<BaseResponse<BankTransactionDTO>>
{
    public Guid Id { get; set; }

    public GetBankTransactionByIdQuery(Guid id)
    {
        Id = id;
    }
}

public class GetTransactionByIdHandler : IRequestHandler<GetBankTransactionByIdQuery, BaseResponse<BankTransactionDTO>>
{
    private readonly IRepository<BankTransaction> _repository;
    private readonly IMapper _mapper;

    public GetTransactionByIdHandler(IRepository<BankTransaction> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<BankTransactionDTO>> Handle(GetBankTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<BankTransactionDTO>();

        try
        {
            var result = await _repository.GetByIdAsync(request.Id);

            if (result == null)
            {
                response.SetReturnErrorStatus("Result not found");
            }
            else
            {
                response.Data = _mapper.Map<BankTransactionDTO>(result);
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
