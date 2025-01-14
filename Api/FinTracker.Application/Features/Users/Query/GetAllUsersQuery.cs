using AutoMapper;
using FinTracker.Application.Common;
using FinTracker.Application.Models.DTOs;
using FinTracker.Domain.Entities;
using MediatR;
using FinTracker.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Application.Features.Users.Query;

public class GetAllUsersQuery : IRequest<BaseResponse<List<UserDTO>>>
{
}

public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, BaseResponse<List<UserDTO>>>
{
    private readonly IRepository<User> _repository;
    private readonly IMapper _mapper;

    public GetAllUsersHandler(IRepository<User> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<List<UserDTO>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<List<UserDTO>>();

        try
        {
            var result = await _repository.GetAllAsync();
            var data = _mapper.Map<List<UserDTO>>(result);

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
