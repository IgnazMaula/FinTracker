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
public class AccountRepository : IAccountRepository
{
    private readonly DatabaseContext _context;

    public AccountRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Account>> GetAllAccountsAsync()
    {
        return await _context.Accounts.ToListAsync();
    }

    public async Task<Account> GetAccountByIdAsync(Guid projectId)
    {
        return await _context.Accounts.FirstOrDefaultAsync(p => p.Id == projectId);
    }

    public async Task CreateAccountAsync(Account project)
    {
        await _context.Accounts.AddAsync(project);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAccountAsync(Account project)
    {
        _context.Accounts.Update(project);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAccountAsync(Guid projectId)
    {
        var project = await _context.Accounts.FindAsync(projectId);
        if (project != null)
        {
            _context.Accounts.Remove(project);
            await _context.SaveChangesAsync();
        }
    }
}
