using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Domain.Entities;

public class BankAccount
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public string Name { get; set; }
    public string AccountNo { get; set; }
    public string BankName { get; set; }
    public string Currency { get; set; }
    public decimal InitialBalance { get; set; } = 0;
    public decimal CurrentBalance { get; set; } = 0;
}
