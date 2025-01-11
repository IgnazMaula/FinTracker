using AutoMapper;
using MediatR;
using FinTracker.Application.Common;
using FinTracker.Application.DTOs;
using FinTracker.Domain.Entities;
using FinTracker.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

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
    private readonly IAccountRepository _repository;
    private readonly IMapper _mapper;

    public UpdateAccountHandler(IAccountRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<AccountDTO>> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<AccountDTO>();

        try
        {
            var user = await _repository.GetAccountByIdAsync(request.Id);
            if (user == null)
            {
                response.SetReturnErrorStatus("Account not found");
                return response;
            }

            user.Name = request.Name;
            user.Type = request.Type;
            user.Institution = request.Institution;
            user.Description = request.Description;

            await _repository.UpdateAccountAsync(user);

            var updatedAccountDto = _mapper.Map<AccountDTO>(user);
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
