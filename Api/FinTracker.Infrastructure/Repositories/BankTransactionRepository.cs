using FinTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using FinTracker.Domain.Entities;
using FinTracker.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Infrastructure.Repositories;
public class BankTransactionRepository : Repository<BankTransaction>, IBankTransactionRepository
{
    private readonly DatabaseContext _context;

    public BankTransactionRepository(DatabaseContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<BankTransaction>> GetBankTransactionByBankIdAsync(Guid id)
    {
        return await _context.Set<BankTransaction>().Where(w => w.BankAccountId == id).OrderBy(o => o.TransactionDate).Reverse().Take(50).ToListAsync();
    }

    public async Task<List<BankTransaction>> GetBankTransactionByUserIdAsync(Guid id)
    {
        return await _context.Set<BankTransaction>().Where(w => w.BankAccount.UserId == id).OrderBy(o => o.TransactionDate).Reverse().ToListAsync();
    }
}
