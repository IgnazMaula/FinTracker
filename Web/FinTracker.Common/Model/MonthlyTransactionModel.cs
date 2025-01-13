using FinTracker.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Common.Model
{
    public class MonthlyTransactionModel
    {
        public string Month { get; set; }
        public decimal Income { get; set; }
        public decimal Expense { get; set; }

    }
}
