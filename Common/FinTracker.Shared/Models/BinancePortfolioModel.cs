using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Common.Shared.Model;

public class BinancePortfolioModel
{
    public decimal TotalPortfolioValueInUSDT { get; set; }
    public List<AssetBalanceModel> Balances { get; set; }
    public List<TradeModel> Trades { get; set; } = new();
    public List<OrderModel> Orders { get; set; } = new();
    public InvestmentSummaryModel InvestmentSummary { get; set; } = new();
    public List<PortfolioHistoryPoint> PortfolioHistory { get; set; }
}

public class AssetBalanceModel
{
    public string Asset { get; set; }
    public decimal Free { get; set; }
    public decimal Locked { get; set; }
    public decimal ValueInUSDT { get; set; }
}

public class BinanceAccountResponse
{
    public List<BinanceBalance> balances { get; set; }
}

public class BinanceBalance
{
    public string asset { get; set; }
    public string free { get; set; }
    public string locked { get; set; }
}

public class BinancePriceModel
{
    public string symbol { get; set; }
    public string price { get; set; }
}

public class TradeModel
{
    public string Symbol { get; set; }
    public decimal Price { get; set; }
    public decimal Qty { get; set; }
    public decimal QuoteQty { get; set; }
    public bool IsBuyer { get; set; }
    public long Time { get; set; }
}

public class OrderModel
{
    public string Symbol { get; set; }
    public long OrderId { get; set; }
    public string Status { get; set; }
    public decimal OrigQty { get; set; }
    public decimal ExecutedQty { get; set; }
    public string Side { get; set; }
    public long Time { get; set; }
}

public class InvestmentSummaryModel
{
    public decimal GainLoss { get; set; }
    public string BestPerformer { get; set; }
    public decimal DollarGain30d { get; set; }
    public decimal PercentGain30d { get; set; }
    public Dictionary<string, decimal> AllocationPercentage { get; set; } = new();
}

public class PortfolioHistoryPoint
{
    public string Date { get; set; }
    public decimal TotalValueInUSDT { get; set; }
}