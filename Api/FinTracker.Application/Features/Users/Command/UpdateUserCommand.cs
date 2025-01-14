using AutoMapper;
using MediatR;
using FinTracker.Application.Common;
using FinTracker.Application.Models.DTOs;
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
    private readonly IRepository<User> _repository;
    private readonly IMapper _mapper;

    public UpdateUserHandler(IRepository<User> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<UserDTO>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<UserDTO>();

        try
        {
            var user = await _repository.GetByIdAsync(request.Id);
            if (user == null)
            {
                response.SetReturnErrorStatus("User not found");
                return response;
            }

            user.Username = request.Username;
            user.Email = request.Email;
            user.PasswordHash = request.PasswordHash;
            user.Role = request.Role;

            await _repository.UpdateAsync(user);

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
