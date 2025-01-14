using AutoMapper;
using FinTracker.Application.Common;
using FinTracker.Domain.Entities;
using MediatR;
using FinTracker.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using FinTracker.Application.Models.DTOs;

namespace FinTracker.Application.Features.Accounts.Query;

public class GetAccountByIdQuery : IRequest<BaseResponse<AccountDTO>>
{
    public Guid Id { get; set; }

    public GetAccountByIdQuery(Guid id)
    {
        Id = id;
    }
}

public class GetAccountByIdHandler : IRequestHandler<GetAccountByIdQuery, BaseResponse<AccountDTO>>
{
    private readonly IRepository<Account> _repository;
    private readonly IMapper _mapper;

    public GetAccountByIdHandler(IRepository<Account> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<AccountDTO>> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<AccountDTO>();

        try
        {
            var user = await _repository.GetByIdAsync(request.Id);

            if (user == null)
            {
                response.SetReturnErrorStatus("Account not found");
            }
            else
            {
                response.Data = _mapper.Map<AccountDTO>(user);
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
