using FinTracker.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Common.Model;

public class InvestmentAccountModel
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public UserModel? User { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string AccountNo { get; set; }
    [Required]
    public string InvestmentName { get; set; }
    [Required]
    public string Currency { get; set; }
    public decimal InitialBalance { get; set; }
    public decimal CurrentBalance { get; set; }
}
