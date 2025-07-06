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
        public Guid BankAccountId { get; set; }
        public string Period { get; set; } = "";
        public decimal TotalCredit { get; set; }
        public decimal TotalDebit { get; set; }
        public decimal SurplusDeficit { get; set; }

    }
}
