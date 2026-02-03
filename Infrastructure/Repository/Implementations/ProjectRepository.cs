using Microsoft.EntityFrameworkCore;
using PortfolioApi.Domain.Entities;
using PortfolioApi.Infrastructure.Data;
using PortfolioApi.Infrastructure.Repository.Interfaces;

namespace PortfolioApi.Infrastructure.Repository.Implementations;

public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _context;

    public ProjectRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        
        var teste = await _context.Projects
            .Include(p => p.Technologies)
                .ThenInclude(t => t.Icon)
            .Include(p => p.User)
            .AsNoTracking()
            .ToListAsync();

        return teste;
    }

    public async Task<Project?> GetByIdAsync(int projectId)
    {
        var project = await _context.Projects
            .Include(p => p.Technologies)
                .ThenInclude(t => t.Icon)
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == projectId);

        return project;
    }
    public async Task<int> AddAsync(Project project)
    {
        foreach (var tech in project.Technologies)
        {
            tech.Icon = null; 
            _context.Attach(tech);
        }
        
        foreach (var tech in project.Technologies)
        {
            tech.Icon = null; 
            _context.Attach(tech);
        }

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return project.Id;
    }

    public  async Task UpdateAsync(Project project)
    {
         _context.Update(project);
         await _context.SaveChangesAsync();
         
    }

    public async Task<bool> DeleteAsync(int projectId)
    {
        var project = await _context.Projects.FindAsync(projectId);
        if (project == null) return false;

        _context.Projects.Remove(project);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<Project>> GetByUserIdAsync(int userId)
    {
        var projects = await _context.Projects
            .Where(p => p.UserId == userId)
            .Include(p => p.Technologies)
                .ThenInclude(t => t.Icon)
            .Include(p => p.User)
            .ToListAsync();
        
        return projects;
    }

    public async Task<List<Project>> GetProjectsByUserIdAndProjectIdsAsync(int userId, List<int> projectIds)
    {
        return await _context.Projects.Where(p => p.UserId == userId && projectIds.Contains(p.Id))
            .ToListAsync();
    }

    public async Task RemoveAllTechnologiesFromProject(int projectId)
    {
        var project = await _context.Projects
            .Include(p => p.Technologies)
            .FirstOrDefaultAsync(p => p.Id == projectId);

        if (project != null)
        {
            project.Technologies.Clear();
            await _context.SaveChangesAsync();
        }
    }

}
