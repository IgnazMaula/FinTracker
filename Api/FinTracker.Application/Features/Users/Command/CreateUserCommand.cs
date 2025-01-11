using AutoMapper;
using FinTracker.Domain.Entities;
using MediatR;
using FinTracker.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using FinTracker.Application.DTOs;
using FinTracker.Application.Common;

namespace FinTracker.Application.Features.Users.Command;

public class CreateUserCommand : IRequest<BaseResponse<UserDTO>>
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; }
}

public class CreateUserHandler : IRequestHandler<CreateUserCommand, BaseResponse<UserDTO>>
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;

    public CreateUserHandler(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<UserDTO>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<UserDTO>();

        try
        {
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Email = request.Email,
                PasswordHash = request.PasswordHash,
                Role = request.Role,
            };

            await _repository.CreateUserAsync(newUser);
            var projectDto = _mapper.Map<UserDTO>(newUser);

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
