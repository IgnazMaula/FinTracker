using FinTracker.Application.Models;
using FinTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Application.Interfaces
{
    public interface ICMCService
    {
        Task<List<CoinPrice>> GetCryptoChartData();
    }
}
