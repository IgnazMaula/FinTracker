using FinTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Application.Models.DTOs;

public class BankTransactionDTO
{
    public Guid Id { get; set; }
    public Guid BankAccountId { get; set; }
    public BankAccountDTO? BankAccount { get; set; }
    public DateTime TransactionDate { get; set; }
    public string Description { get; set; }
    public string TransactionType { get; set; }
    public decimal TransactionAmount { get; set; }
    public decimal Balance { get; set; }
}
