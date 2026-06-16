namespace FinTracker.Application.Models.Requests;

public class DCACalculatorRequest
{
    public string Ticker { get; set; } = string.Empty;
    public double InitialInvestment { get; set; }
    public double RecurringInvestment { get; set; }
    public string StartMonth { get; set; } = string.Empty;
    public string EndMonth { get; set; } = string.Empty;
    public int StartYear { get; set; }
    public int EndYear { get; set; }
}
