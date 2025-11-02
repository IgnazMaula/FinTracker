using FinTracker.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Common.Model;

public class InvestmentTransactionModel
{
    public Guid Id { get; set; }
    public Guid InvestmentAccountId { get; set; }
    public BankAccountModel? BankAccount { get; set; }
    public DateTime TransactionDate { get; set; } = DateTime.Now;
    public string Description { get; set; }
    public string TransactionType { get; set; }
    public decimal TransactionAmount { get; set; }
}
