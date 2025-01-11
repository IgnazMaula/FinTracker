using AutoMapper;
using FinTracker.Application.Common;
using FinTracker.Application.DTOs;
using FinTracker.Domain.Entities;
using MediatR;
using FinTracker.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FinTracker.Application.Features.Projects.Command;

public class UpdateProjectCommand : IRequest<BaseResponse<ProjectDTO>>
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public Guid AssignedToUserId { get; set; }
    public DateTime DueDate { get; set; }
}

public class UpdateProjectHandler : IRequestHandler<UpdateProjectCommand, BaseResponse<ProjectDTO>>
{
    private readonly IProjectRepository _repository;
    private readonly IMapper _mapper;

    public UpdateProjectHandler(IProjectRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<ProjectDTO>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<ProjectDTO>();

        try
        {
            var project = await _repository.GetProjectByIdAsync(request.Id);
            if (project == null)
            {
                response.SetReturnErrorStatus("Project not found");
                return response;
            }

            project.Title = request.Title;
            project.Description = request.Description;
            project.Status = request.Status;
            project.AssignedToUserId = request.AssignedToUserId;
            project.DueDate = request.DueDate;
            project.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateProjectAsync(project);

            var updatedProjectDto = _mapper.Map<ProjectDTO>(project);
            response.Data = updatedProjectDto;
            response.SetReturnSuccessStatus();
        }
        catch (Exception ex)
        {
            response.SetReturnErrorStatus($"Error: {ex.Message}");
        }

        return response;
    }
}
