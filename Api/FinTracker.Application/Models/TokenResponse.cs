using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Application.Models;

public class TokenResponse
{
    public string Token { get; set; }
    public Guid UserId { get; set; }
    public string Username { get; set; }
}
