using FinTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Domain.Interfaces
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAllAccountsAsync();
        Task<Account> GetAccountByIdAsync(Guid projectId);
        Task CreateAccountAsync(Account project);
        Task UpdateAccountAsync(Account project);
        Task DeleteAccountAsync(Guid projectId);
    }
}
