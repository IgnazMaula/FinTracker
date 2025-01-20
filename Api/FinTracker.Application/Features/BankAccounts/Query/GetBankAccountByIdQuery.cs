using AutoMapper;
using FinTracker.Application.Common;
using FinTracker.Domain.Entities;
using MediatR;
using FinTracker.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using FinTracker.Application.Models.DTOs;

namespace FinTracker.Application.Features.BankAccounts.Query;

public class GetBankAccountByIdQuery : IRequest<BaseResponse<BankAccountDTO>>
{
    public Guid Id { get; set; }

    public GetBankAccountByIdQuery(Guid id)
    {
        Id = id;
    }
}

public class GetAccountByIdHandler : IRequestHandler<GetBankAccountByIdQuery, BaseResponse<BankAccountDTO>>
{
    private readonly IRepository<BankAccount> _repository;
    private readonly IMapper _mapper;

    public GetAccountByIdHandler(IRepository<BankAccount> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<BankAccountDTO>> Handle(GetBankAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<BankAccountDTO>();

        try
        {
            var user = await _repository.GetByIdAsync(request.Id);

            if (user == null)
            {
                response.SetReturnErrorStatus("Account not found");
            }
            else
            {
                response.Data = _mapper.Map<BankAccountDTO>(user);
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
