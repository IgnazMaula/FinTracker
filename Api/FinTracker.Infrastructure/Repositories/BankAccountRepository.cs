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
public class BankAccountRepository : Repository<BankAccount>, IBankAccountRepository
{
    private readonly DatabaseContext _context;

    public BankAccountRepository(DatabaseContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<BankAccount>> GetBankAccountByUserIdAsync(Guid id)
    {
        return await _context.Set<BankAccount>().Where(w => w.UserId == id).ToListAsync();
    }
}
