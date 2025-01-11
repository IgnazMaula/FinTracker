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
public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> GetUserByIdAsync(Guid projectId)
    {
        return await _context.Users.FirstOrDefaultAsync(p => p.Id == projectId);
    }

    public async Task CreateUserAsync(User project)
    {
        await _context.Users.AddAsync(project);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(User project)
    {
        _context.Users.Update(project);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(Guid projectId)
    {
        var project = await _context.Users.FindAsync(projectId);
        if (project != null)
        {
            _context.Users.Remove(project);
            await _context.SaveChangesAsync();
        }
    }
}
