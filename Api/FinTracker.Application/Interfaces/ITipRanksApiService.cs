using FinTracker.Application.Models;
using FinTracker.Application.Models.DTOs;
using FinTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Application.Interfaces
{
    public interface ITipRanksApiService
    {
        Task<List<DCAHistoryDataDTO>> GetHistoricalPrice(string ticker);
    }
}
