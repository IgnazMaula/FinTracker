using AutoMapper;
using FinTracker.Application.Common;
using FinTracker.Domain.Entities;
using MediatR;
using FinTracker.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using FinTracker.Application.Models.DTOs;

namespace FinTracker.Application.Features.Dashboard.Query;

public class GetCashflowDashboardQuery : IRequest<BaseResponse<CashflowDashboardDTO>>
{
    public Guid Id { get; set; }

    public GetCashflowDashboardQuery(Guid id)
    {
        Id = id;
    }
}

public class GetCashflowDashboardQueryHandler : IRequestHandler<GetCashflowDashboardQuery, BaseResponse<CashflowDashboardDTO>>
{
    private readonly IRepository<User> _userRepository;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IBankTransactionRepository _bankTransactionRepository;
    private readonly IMapper _mapper;

    public GetCashflowDashboardQueryHandler(IRepository<User> userRepository, IBankAccountRepository bankAccountRepository, IBankTransactionRepository bankTransactionRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _bankAccountRepository = bankAccountRepository;
        _bankTransactionRepository = bankTransactionRepository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<CashflowDashboardDTO>> Handle(GetCashflowDashboardQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<CashflowDashboardDTO>();

        try
        {
            var result = new CashflowDashboardDTO();

            


            if (result == null)
            {
                response.SetReturnErrorStatus("Result not found");
            }
            else
            {
                response.Data = _mapper.Map<CashflowDashboardDTO>(result);
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
