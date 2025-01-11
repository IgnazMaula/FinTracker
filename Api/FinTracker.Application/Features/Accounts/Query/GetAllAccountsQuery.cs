using AutoMapper;
using FinTracker.Application.Common;
using FinTracker.Application.DTOs;
using FinTracker.Domain.Entities;
using MediatR;
using FinTracker.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Application.Features.Accounts.Query;

public class GetAllAccountsQuery : IRequest<BaseResponse<List<AccountDTO>>>
{
}

public class GetAllAccountsHandler : IRequestHandler<GetAllAccountsQuery, BaseResponse<List<AccountDTO>>>
{
    private readonly IAccountRepository _repository;
    private readonly IMapper _mapper;

    public GetAllAccountsHandler(IAccountRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<List<AccountDTO>>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<List<AccountDTO>>();

        try
        {
            var result = await _repository.GetAllAccountsAsync();
            var data = _mapper.Map<List<AccountDTO>>(result);

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
