using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Application.DTOs;

public class AccountDTO
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string? Institution { get; set; }
    public string? Description { get; set; }
    public decimal InitialBalance { get; set; }
    public decimal CurrentBalance { get; set; }
}
