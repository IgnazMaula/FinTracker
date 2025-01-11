using AutoMapper;
using FinTracker.Application.Common;
using FinTracker.Application.DTOs;
using FinTracker.Domain.Entities;
using MediatR;
using FinTracker.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FinTracker.Application.Features.Projects.Query;

public class GetProjectByIdQuery : IRequest<BaseResponse<ProjectDTO>>
{
    public Guid Id { get; set; }

    public GetProjectByIdQuery(Guid id)
    {
        Id = id;
    }
}

public class GetProjectByIdHandler : IRequestHandler<GetProjectByIdQuery, BaseResponse<ProjectDTO>>
{
    private readonly IProjectRepository _repository;
    private readonly IMapper _mapper;

    public GetProjectByIdHandler(IProjectRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<ProjectDTO>> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<ProjectDTO>();

        try
        {
            var project = await _repository.GetProjectByIdAsync(request.Id);

            if (project == null)
            {
                response.SetReturnErrorStatus("Project not found");
            }
            else
            {
                response.Data = _mapper.Map<ProjectDTO>(project);
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
