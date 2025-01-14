using MediatR;
using FinTracker.Application.Common;
using FinTracker.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using FinTracker.Domain.Entities;

namespace FinTracker.Application.Features.Accounts.Command;

public class DeleteAccountCommand : IRequest<BaseResponse<bool>>
{
    public Guid Id { get; set; }

    public DeleteAccountCommand(Guid id)
    {
        Id = id;
    }
}

public class DeleteAccountHandler : IRequestHandler<DeleteAccountCommand, BaseResponse<bool>>
{
    private readonly IRepository<Account> _repository;

    public DeleteAccountHandler(IRepository<Account> repository)
    {
        _repository = repository;
    }

    public async Task<BaseResponse<bool>> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var project = await _repository.GetByIdAsync(request.Id);
            if (project == null)
            {
                response.SetReturnErrorStatus("Account not found");
                response.Data = false;
                return response;
            }

            await _repository.DeleteAsync(request.Id);

            response.Data = true;
            response.SetReturnSuccessStatus();
        }
        catch (Exception ex)
        {
            response.SetReturnErrorStatus($"Error: {ex.Message}");
            response.Data = false;
        }

        return response;
    }
}
