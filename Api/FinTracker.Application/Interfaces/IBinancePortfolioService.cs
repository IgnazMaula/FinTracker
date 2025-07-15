using FinTracker.Application.Models;
using FinTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinTracker.Common.Shared.Model;

namespace FinTracker.Application.Interfaces
{
    public interface IBinancePortfolioService
    {
        Task<BinancePortfolioModel> GetPortfolioAsync();
    }
}
