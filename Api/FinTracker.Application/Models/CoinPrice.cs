using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Application.Models;

public class CoinPrice
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Symbol { get; set; }
    public decimal Price { get; set; }

    public decimal PercentChange1h { get; set; }
    public decimal PercentChange24h { get; set; }
    public decimal PercentChange7d { get; set; }

    public decimal MarketCap { get; set; }
    public decimal Volume24h { get; set; }

    public decimal CirculatingSupply { get; set; }

    public string LogoUrl { get; set; }

    public List<decimal> Sparkline7d { get; set; } = new();
}

