using FinTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using FinTracker.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinTracker.Infrastructure.Persistence;

namespace FinTracker.Infrastructure.Repositories;
public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _context;

    public ProjectRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Project>> GetAllProjectsAsync()
    {
        return await _context.Projects.ToListAsync();
    }

    public async Task<Project> GetProjectByIdAsync(Guid projectId)
    {
        return await _context.Projects.Include(p => p.AssignedToUser).FirstOrDefaultAsync(p => p.Id == projectId);
    }

    public async Task<IEnumerable<Project>> GetProjectsByUserIdAsync(Guid userId)
    {
        return await _context.Projects.Where(p => p.AssignedToUserId == userId).ToListAsync();
    }

    public async Task CreateProjectAsync(Project project)
    {
        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateProjectAsync(Project project)
    {
        _context.Projects.Update(project);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteProjectAsync(Guid projectId)
    {
        var project = await _context.Projects.FindAsync(projectId);
        if (project != null)
        {
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }
    }
}
