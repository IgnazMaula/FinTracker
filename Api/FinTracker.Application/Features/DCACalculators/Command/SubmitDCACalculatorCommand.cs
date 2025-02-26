using AutoMapper;
using FinTracker.Domain.Entities;
using MediatR;
using FinTracker.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using FinTracker.Application.Models.DTOs;
using FinTracker.Application.Common;
using FinTracker.Application.Interfaces;

namespace FinTracker.Application.Features.DCACalculators.Command;

public class SubmitDCACalculatorCommand : IRequest<BaseResponse<List<DCAResultDTO>>>
{
    public string Ticker { get; set; }
    public double InitialInvestment { get; set; }
    public double RecurringInvestment { get; set; }
    public string StartMonth { get; set; }
    public string EndMonth { get; set; }
    public int StartYear { get; set; }
    public int EndYear { get; set; }
}

public class SubmitDCACalculatorHandler : IRequestHandler<SubmitDCACalculatorCommand, BaseResponse<List<DCAResultDTO>>>
{
    private readonly ITipRanksApiService _tipRanksApiServuce;
    private readonly IMapper _mapper;

    public SubmitDCACalculatorHandler(ITipRanksApiService tipRanksApiServuce, IMapper mapper)
    {
        _tipRanksApiServuce = tipRanksApiServuce;
        _mapper = mapper;
    }

    public async Task<BaseResponse<List<DCAResultDTO>>> Handle(SubmitDCACalculatorCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<List<DCAResultDTO>>();

        try
        {
            var result = new List<DCAResultDTO>();

            var historicalPrices = await _tipRanksApiServuce.GetHistoricalPrice(request.Ticker);

            double totalInvested = request.InitialInvestment;
            double lastTotal = 0;
            double lastPrice = 0;
            for (int year = request.StartYear; year <= request.EndYear; year++)
            {
                for (int month = 1; month <= 12; month++)
                {
                    var theDate = new DateTime(year, month, 1);
                    var theDate2 = new DateTime(year, month, 2);
                    var theDate3 = new DateTime(year, month, 3);
                    var historicalPrice = historicalPrices.Where(W => W.Date == theDate).FirstOrDefault();
                    if(historicalPrice == null)
                    {
                        historicalPrice = historicalPrices.Where(W => W.Date == theDate2).FirstOrDefault();
                    }
                    if (historicalPrice == null)
                    {
                        historicalPrice = historicalPrices.Where(W => W.Date == theDate3).FirstOrDefault();
                    }
                    if (historicalPrice != null)
                    {
                        if (result.Count == 0) { lastPrice = historicalPrice.Price; }
                        var totalGained = (lastTotal + (result.Count == 0 ? request.RecurringInvestment : 0))
                        / lastPrice * (historicalPrice.Price - lastPrice);
                        totalInvested += request.RecurringInvestment;
                        var total = totalInvested + totalGained;
                        var percentGain = (totalGained / totalInvested) * 100;
                        lastPrice = historicalPrice.Price;
                        lastTotal = total;

                        result.Add(new DCAResultDTO
                        {
                            Date = historicalPrice.Date,
                            Price = historicalPrice.Price,
                            PercentGain = percentGain,
                            TotalInvested = totalInvested,
                            TotalGain = total - totalInvested,
                            Total = total
                        });
                    }
                }

            }

            //var dcaResult = new BankAccount
            //{
            //    Id = Guid.NewGuid(),
            //    UserId = request.UserId,
            //    User = user,
            //    Name = request.Name,
            //    BankName = request.BankName,
            //    AccountNo = request.AccountNo,
            //    Currency = request.Currency,
            //    InitialBalance = request.InitialBalance,
            //    CurrentBalance = request.InitialBalance
            //};

            //await _accountRepository.CreateAsync(dcaResult);
            //var projectDto = _mapper.Map<List<DCAResultDTO>>(dcaResult);

            response.Data = result;
            response.SetReturnSuccessStatus();
        }
        catch (Exception ex)
        {
            response.SetReturnErrorStatus($"Error: {ex.Message}");
        }

        return response;
    }
}
