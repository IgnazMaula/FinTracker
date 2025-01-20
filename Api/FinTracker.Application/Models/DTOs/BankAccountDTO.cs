using FinTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Application.Models.DTOs;

public class BankAccountDTO
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public UserDTO? User { get; set; }
    public string Name { get; set; }
    public string AccountNo { get; set; }
    public string BankName { get; set; }
    public string Currency { get; set; }
    public decimal InitialBalance { get; set; }
    public decimal CurrentBalance { get; set; }
}
