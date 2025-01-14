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
public class UserRepository : Repository<User>, IUserRepository
{
    private readonly DatabaseContext _context;

    public UserRepository(DatabaseContext context) : base(context)
    {
        _context = context;
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        return await _context.Set<User>().FirstOrDefaultAsync(u => u.Username == username);
    }
}
