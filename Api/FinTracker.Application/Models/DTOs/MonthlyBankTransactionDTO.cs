using FinTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Application.Models.DTOs;

public class MonthlyBankTransactionDTO
{
    public List<MonthlyBankAccountTransaction> MonthlyBankAccountTransaction { get; set; }
    public decimal MontlyTotalCredit { get; set; }
    public decimal MontlyTotalDebit { get; set; }
}

public class MonthlyBankAccountTransaction
{
    public Guid BankAccountId { get; set; }
    public string Period { get; set; }
    public decimal TotalCredit { get; set; }
    public decimal TotalDebit { get; set; }
    public decimal SurplusDeficit { get; set; }
}
