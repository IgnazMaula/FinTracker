using AutoMapper;
using FinTracker.Domain.Entities;
using MediatR;
using FinTracker.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using FinTracker.Application.Models.DTOs;
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
    private readonly IRepository<Account> _accountRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IMapper _mapper;

    public CreateAccountHandler(IRepository<Account> repository, IRepository<User> userRepository, IMapper mapper)
    {
        _accountRepository = repository;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<BaseResponse<AccountDTO>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<AccountDTO>();

        try
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);

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

            await _accountRepository.CreateAsync(newAccount);
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
