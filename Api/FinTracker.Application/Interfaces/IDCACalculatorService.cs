using FinTracker.Application.Common;
using FinTracker.Application.Models.DTOs;
using FinTracker.Application.Models.Requests;

namespace FinTracker.Application.Interfaces;

public interface IDCACalculatorService
{
    Task<BaseResponse<List<DCAResultDTO>>> SubmitAsync(DCACalculatorRequest request);
}
