using AutoMapper;
using FinTracker.Application.Common;
using FinTracker.Application.Interfaces;
using FinTracker.Application.Models.DTOs;

namespace FinTracker.Application.Services;

public class CashflowService : ICashflowService
{
    private readonly IMapper _mapper;

    public CashflowService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public Task<BaseResponse<CashflowDashboardDTO>> GetDashboardAsync(Guid userId)
    {
        var response = new BaseResponse<CashflowDashboardDTO>();

        try
        {
            var result = new CashflowDashboardDTO();
            response.Data = _mapper.Map<CashflowDashboardDTO>(result);
            response.SetReturnSuccessStatus();
        }
        catch (Exception ex)
        {
            response.SetReturnErrorStatus($"Error: {ex.Message}");
        }

        return Task.FromResult(response);
    }
}
