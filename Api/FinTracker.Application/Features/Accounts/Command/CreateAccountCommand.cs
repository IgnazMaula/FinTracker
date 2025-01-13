using AutoMapper;
using FinTracker.Domain.Entities;
using MediatR;
using FinTracker.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using FinTracker.Application.DTOs;
using FinTracker.Application.Common;

namespace FinTracker.Application.Features.Accounts.Command;

public class CreateAccountCommand : IRequest<BaseResponse<AccountDTO>>
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string Institution { get; set; }
    public string Description { get; set; }
    public decimal InitialBalance { get; set; }
    public decimal CurrentBalance { get; set; }
    public Guid UserId { get; set; }
}

public class CreateAccountHandler : IRequestHandler<CreateAccountCommand, BaseResponse<AccountDTO>>
{
    private readonly IAccountRepository _repository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public CreateAccountHandler(IAccountRepository repository, IMapper mapper, IUserRepository userRepository)
    {
        _repository = repository;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<BaseResponse<AccountDTO>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<AccountDTO>();

        try
        {
            var user = await _userRepository.GetUserByIdAsync(request.UserId);

            var newAccount = new Account
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                User = user,
                Name = request.Name,
                Type = request.Type,
                Institution = request.Institution,
                Description = request.Description,
                InitialBalance = request.InitialBalance,
                CurrentBalance = request.CurrentBalance,
            };

            await _repository.CreateAccountAsync(newAccount);
            var projectDto = _mapper.Map<AccountDTO>(newAccount);

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
