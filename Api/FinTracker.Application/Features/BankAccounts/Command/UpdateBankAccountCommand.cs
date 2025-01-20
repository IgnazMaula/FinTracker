using AutoMapper;
using MediatR;
using FinTracker.Application.Common;
using FinTracker.Application.Models.DTOs;
using FinTracker.Domain.Entities;
using FinTracker.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using FinTracker.Application.Models.DTOs;

namespace FinTracker.Application.Features.BankAccounts.Command;

public class UpdateBankAccountCommand : IRequest<BaseResponse<BankAccountDTO>>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string AccountNo { get; set; }
    public string BankName { get; set; }
}

public class UpdateAccountHandler : IRequestHandler<UpdateBankAccountCommand, BaseResponse<BankAccountDTO>>
{
    private readonly IRepository<BankAccount> _repository;
    private readonly IMapper _mapper;

    public UpdateAccountHandler(IRepository<BankAccount> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<BankAccountDTO>> Handle(UpdateBankAccountCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<BankAccountDTO>();

        try
        {
            var account = await _repository.GetByIdAsync(request.Id);
            if (account == null)
            {
                response.SetReturnErrorStatus("Account not found");
                return response;
            }

            account.Name = request.Name;
            account.AccountNo = request.AccountNo;
            account.BankName = request.BankName;

            await _repository.UpdateAsync(account);

            var updatedAccountDto = _mapper.Map<BankAccountDTO>(account);
            response.Data = updatedAccountDto;
            response.SetReturnSuccessStatus();
        }
        catch (Exception ex)
        {
            response.SetReturnErrorStatus($"Error: {ex.Message}");
        }

        return response;
    }
}
