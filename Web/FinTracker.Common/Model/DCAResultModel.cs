using FinTracker.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Common.Model;

public class DCAResultModel
{
    public DateTime Date { get; set; }
    public double Price { get; set; }
    public double PercentGain { get; set; }
    public double TotalInvested { get; set; }
    public double TotalGain { get; set; }
    public double Total { get; set; }
}
