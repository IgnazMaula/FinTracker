using AutoMapper;
using FinTracker.Domain.Entities;
using MediatR;
using FinTracker.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using FinTracker.Application.Models.DTOs;
using FinTracker.Application.Common;

namespace FinTracker.Application.Features.BankAccounts.Command;

public class CreateBankAccountCommand : IRequest<BaseResponse<BankAccountDTO>>
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string AccountNo { get; set; }
    public string BankName { get; set; }
    public string Currency { get; set; }
    public decimal InitialBalance { get; set; }
}

public class CreateAccountHandler : IRequestHandler<CreateBankAccountCommand, BaseResponse<BankAccountDTO>>
{
    private readonly IRepository<BankAccount> _accountRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IMapper _mapper;

    public CreateAccountHandler(IRepository<BankAccount> repository, IRepository<User> userRepository, IMapper mapper)
    {
        _accountRepository = repository;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<BaseResponse<BankAccountDTO>> Handle(CreateBankAccountCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<BankAccountDTO>();

        try
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);

            var newAccount = new BankAccount
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                User = user,
                Name = request.Name,
                BankName = request.BankName,
                AccountNo = request.AccountNo,
                Currency = request.Currency,
                InitialBalance = request.InitialBalance,
                CurrentBalance = request.InitialBalance
            };

            await _accountRepository.CreateAsync(newAccount);
            var projectDto = _mapper.Map<BankAccountDTO>(newAccount);

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
