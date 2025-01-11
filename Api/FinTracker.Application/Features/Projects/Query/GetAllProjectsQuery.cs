using AutoMapper;
using FinTracker.Application.Common;
using FinTracker.Application.DTOs;
using MediatR;
using FinTracker.Domain.Entities;
using FinTracker.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Application.Features.Projects.Query;

public class GetAllProjectsQuery : IRequest<BaseResponse<List<ProjectDTO>>>
{
}

public class GetAllProjectsHandler : IRequestHandler<GetAllProjectsQuery, BaseResponse<List<ProjectDTO>>>
{
    private readonly IProjectRepository _repository;
    private readonly IMapper _mapper;

    public GetAllProjectsHandler(IProjectRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<List<ProjectDTO>>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<List<ProjectDTO>>();

        try
        {
            var result = await _repository.GetAllProjectsAsync();
            var data = _mapper.Map<List<ProjectDTO>>(result);

            response.Data = data;
            response.SetReturnSuccessStatus();
        }
        catch (Exception ex)
        {
            response.SetReturnErrorStatus($"Error: {ex.Message}");
        }

        return response;
    }
}
