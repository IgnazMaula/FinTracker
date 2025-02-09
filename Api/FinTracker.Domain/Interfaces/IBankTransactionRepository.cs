using FinTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Domain.Interfaces
{
    public interface IBankTransactionRepository : IRepository<BankTransaction>
    {
        Task<List<BankTransaction>> GetBankTransactionByBankIdAsync(Guid Id);
        Task<List<BankTransaction>> GetBankTransactionByUserIdAsync(Guid Id);
    }
}
