using FinTracker.Application.Common;
using FinTracker.Application.Models.DTOs;

namespace FinTracker.Application.Interfaces;

public interface ICashflowService
{
    Task<BaseResponse<CashflowDashboardDTO>> GetDashboardAsync(Guid userId);
}
