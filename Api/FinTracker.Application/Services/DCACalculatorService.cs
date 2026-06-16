using FinTracker.Application.Common;
using FinTracker.Application.Interfaces;
using FinTracker.Application.Models.DTOs;
using FinTracker.Application.Models.Requests;

namespace FinTracker.Application.Services;

public class DCACalculatorService : IDCACalculatorService
{
    private readonly ITipRanksApiService _tipRanksApiService;

    public DCACalculatorService(ITipRanksApiService tipRanksApiService)
    {
        _tipRanksApiService = tipRanksApiService;
    }

    public async Task<BaseResponse<List<DCAResultDTO>>> SubmitAsync(DCACalculatorRequest request)
    {
        var response = new BaseResponse<List<DCAResultDTO>>();

        try
        {
            var result = new List<DCAResultDTO>();
            var historicalPrices = await _tipRanksApiService.GetHistoricalPrice(request.Ticker);
            var firstPrice = historicalPrices.FirstOrDefault(w => w.Date == new DateTime(request.StartYear, 1, 1))
                ?? historicalPrices.FirstOrDefault(w => w.Date == new DateTime(request.StartYear, 1, 2))
                ?? historicalPrices.FirstOrDefault(w => w.Date == new DateTime(request.StartYear, 1, 3));

            if (firstPrice == null)
            {
                response.SetReturnErrorStatus("Historical price not found");
                return response;
            }

            var initialInvestment = request.InitialInvestment == 0 ? 1000 : request.InitialInvestment;
            var monthlyInvestment = request.RecurringInvestment == 0 ? 100 : request.RecurringInvestment;
            var totalInvestment = initialInvestment;
            var totalUnits = initialInvestment / firstPrice.Price;

            for (var year = request.StartYear; year <= request.EndYear; year++)
            {
                for (var month = 1; month <= 12; month++)
                {
                    var historicalPrice = historicalPrices.FirstOrDefault(w => w.Date == new DateTime(year, month, 1))
                        ?? historicalPrices.FirstOrDefault(w => w.Date == new DateTime(year, month, 2))
                        ?? historicalPrices.FirstOrDefault(w => w.Date == new DateTime(year, month, 3));

                    if (historicalPrice == null)
                    {
                        continue;
                    }

                    totalUnits += monthlyInvestment / historicalPrice.Price;
                    totalInvestment += monthlyInvestment;

                    var totalValue = totalUnits * historicalPrice.Price;
                    var totalGain = totalValue - totalInvestment;
                    var percentGain = totalGain / totalInvestment * 100;

                    result.Add(new DCAResultDTO
                    {
                        Date = historicalPrice.Date,
                        Price = historicalPrice.Price,
                        PercentGain = percentGain,
                        TotalInvested = totalInvestment,
                        TotalGain = totalGain,
                        Total = totalValue
                    });
                }
            }

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
