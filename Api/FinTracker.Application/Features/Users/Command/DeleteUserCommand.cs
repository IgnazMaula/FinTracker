using MediatR;
using FinTracker.Application.Common;
using FinTracker.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FinTracker.Application.Features.Users.Command;

public class DeleteUserCommand : IRequest<BaseResponse<bool>>
{
    public Guid Id { get; set; }

    public DeleteUserCommand(Guid id)
    {
        Id = id;
    }
}

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, BaseResponse<bool>>
{
    private readonly IUserRepository _repository;

    public DeleteUserHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<BaseResponse<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var project = await _repository.GetUserByIdAsync(request.Id);
            if (project == null)
            {
                response.SetReturnErrorStatus("User not found");
                response.Data = false;
                return response;
            }

            await _repository.DeleteUserAsync(request.Id);

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
