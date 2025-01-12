using FinTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Application.Interfaces
{
    public interface IAuthService
    {
        Task<TokenResponse> AuthenticateAsync(string username, string password);
    }
}
