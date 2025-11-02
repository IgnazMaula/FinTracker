using FinTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Application.Models.DTOs;

public class CashflowDashboardDTO
{
    public double? TotalAmount { get; set; }
    public int? BankAccountCount { get; set; }
    public double? Percent { get; set; }

    public List<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();
    public List<BankTransactionDTO> BankTransactions { get; set; } = new List<BankTransactionDTO>();
}
