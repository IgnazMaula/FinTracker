using FinTracker.Application.Common;
using MediatR;
using FinTracker.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FinTracker.Application.Features.Projects.Command;

public class DeleteProjectCommand : IRequest<BaseResponse<bool>>
{
    public Guid Id { get; set; }

    public DeleteProjectCommand(Guid id)
    {
        Id = id;
    }
}

public class DeleteProjectHandler : IRequestHandler<DeleteProjectCommand, BaseResponse<bool>>
{
    private readonly IProjectRepository _repository;

    public DeleteProjectHandler(IProjectRepository repository)
    {
        _repository = repository;
    }

    public async Task<BaseResponse<bool>> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var project = await _repository.GetProjectByIdAsync(request.Id);
            if (project == null)
            {
                response.SetReturnErrorStatus("Project not found");
                response.Data = false;
                return response;
            }

            await _repository.DeleteProjectAsync(request.Id);

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
