using AutoMapper;
using MediatR;
using FinTracker.Application.Common;
using FinTracker.Application.DTOs;
using FinTracker.Domain.Entities;
using FinTracker.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FinTracker.Application.Features.Users.Command;

public class UpdateUserCommand : IRequest<BaseResponse<UserDTO>>
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; }
}

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, BaseResponse<UserDTO>>
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;

    public UpdateUserHandler(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<UserDTO>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<UserDTO>();

        try
        {
            var user = await _repository.GetUserByIdAsync(request.Id);
            if (user == null)
            {
                response.SetReturnErrorStatus("User not found");
                return response;
            }

            user.Username = request.Username;
            user.Email = request.Email;
            user.PasswordHash = request.PasswordHash;
            user.Role = request.Role;

            await _repository.UpdateUserAsync(user);

            var updatedUserDto = _mapper.Map<UserDTO>(user);
            response.Data = updatedUserDto;
            response.SetReturnSuccessStatus();
        }
        catch (Exception ex)
        {
            response.SetReturnErrorStatus($"Error: {ex.Message}");
        }

        return response;
    }
}
