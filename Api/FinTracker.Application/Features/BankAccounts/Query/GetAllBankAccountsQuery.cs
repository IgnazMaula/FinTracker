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

namespace FinTracker.Application.Features.BankAccounts.Query;

public class GetAllBankAccountsQuery : IRequest<BaseResponse<List<BankAccountDTO>>>
{
}

public class GetAllAccountsHandler : IRequestHandler<GetAllBankAccountsQuery, BaseResponse<List<BankAccountDTO>>>
{
    private readonly IRepository<BankAccount> _repository;
    private readonly IMapper _mapper;

    public GetAllAccountsHandler(IRepository<BankAccount> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<List<BankAccountDTO>>> Handle(GetAllBankAccountsQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<List<BankAccountDTO>>();

        try
        {
            var result = await _repository.GetAllAsync();
            var data = _mapper.Map<List<BankAccountDTO>>(result);

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
