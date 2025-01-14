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

namespace FinTracker.Application.Features.Accounts.Command;

public class UpdateAccountCommand : IRequest<BaseResponse<AccountDTO>>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Institution { get; set; }
    public string Description { get; set; }
}

public class UpdateAccountHandler : IRequestHandler<UpdateAccountCommand, BaseResponse<AccountDTO>>
{
    private readonly IRepository<Account> _repository;
    private readonly IMapper _mapper;

    public UpdateAccountHandler(IRepository<Account> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<AccountDTO>> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<AccountDTO>();

        try
        {
            var account = await _repository.GetByIdAsync(request.Id);
            if (account == null)
            {
                response.SetReturnErrorStatus("Account not found");
                return response;
            }

            account.Name = request.Name;
            account.Type = request.Type;
            account.Institution = request.Institution;
            account.Description = request.Description;

            await _repository.UpdateAsync(account);

            var updatedAccountDto = _mapper.Map<AccountDTO>(account);
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
