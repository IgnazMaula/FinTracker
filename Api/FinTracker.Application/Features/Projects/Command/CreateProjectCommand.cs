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

public class CreateProjectCommand : IRequest<BaseResponse<ProjectDTO>>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public Guid AssignedToUserId { get; set; }
    public DateTime DueDate { get; set; }
}

public class CreateProjectHandler : IRequestHandler<CreateProjectCommand, BaseResponse<ProjectDTO>>
{
    private readonly IProjectRepository _repository;
    private readonly IMapper _mapper;

    public CreateProjectHandler(IProjectRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<ProjectDTO>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<ProjectDTO>();

        try
        {
            var newProject = new Project
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                Status = request.Status,
                AssignedToUserId = request.AssignedToUserId,
                DueDate = request.DueDate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _repository.CreateProjectAsync(newProject);
            var projectDto = _mapper.Map<ProjectDTO>(newProject);

            response.Data = projectDto;
            response.SetReturnSuccessStatus();
        }
        catch (Exception ex)
        {
            response.SetReturnErrorStatus($"Error: {ex.Message}");
        }

        return response;
    }
}
