using AutoMapper;
using FinTracker.Application.Common;
using FinTracker.Application.DTOs;
using FinTracker.Domain.Entities;
using MediatR;
using FinTracker.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FinTracker.Application.Features.Users.Query;

public class GetUserByIdQuery : IRequest<BaseResponse<UserDTO>>
{
    public Guid Id { get; set; }

    public GetUserByIdQuery(Guid id)
    {
        Id = id;
    }
}

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, BaseResponse<UserDTO>>
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;

    public GetUserByIdHandler(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<UserDTO>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<UserDTO>();

        try
        {
            var user = await _repository.GetUserByIdAsync(request.Id);

            if (user == null)
            {
                response.SetReturnErrorStatus("User not found");
            }
            else
            {
                response.Data = _mapper.Map<UserDTO>(user);
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
