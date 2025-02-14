using FinTracker.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Common.Model;

public class DCACalculatorModel
{
    public string Ticker { get; set; }
    public string Period { get; set; }
    public double InitialInvestment { get; set; }
    public double RecurringInvestment { get; set; }
    public string StartMonth { get; set; }
    public string EndMonth { get; set; }
    public int StartYear { get; set; }
    public int EndYear { get; set; }
}
