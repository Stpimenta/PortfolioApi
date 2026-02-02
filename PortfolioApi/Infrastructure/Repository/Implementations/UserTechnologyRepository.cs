using Microsoft.EntityFrameworkCore;
using PortfolioApi.Domain.Entities;
using PortfolioApi.Infrastructure.Data;
using PortfolioApi.Infrastructure.Repository.Interfaces;

namespace PortfolioApi.Infrastructure.Repository.Implementations;

public class UserTechnologyRepository:IUserTechnologyRepository
{
    private readonly AppDbContext _context;

    public UserTechnologyRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserTechProgress>> GetByUserAsync(int userId)
    {
        return await _context.UserTechProgress
            .Include(utp => utp.Tech)
            .ThenInclude(t => t.Icon)
            .Include(utp => utp.Projects)
            .Where(utp => utp.UserId == userId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<UserTechProgress?> GetAsync(int userId, int techId)
    {
        return await _context.UserTechProgress
            .Include(utp => utp.Tech)
                .ThenInclude(t => t.Icon)
             .Include(utp => utp.Tech)
                .ThenInclude(t => t.Icon)
            .Include(utp => utp.Projects)
            .FirstOrDefaultAsync(utp => utp.UserId == userId && utp.TechId == techId);
    }

    public async Task AddAsync(UserTechProgress userTechProgress)
    {
        _context.UserTechProgress.Add(userTechProgress);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(UserTechProgress userTechProgress)
    {
        _context.UserTechProgress.Update(userTechProgress);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int userId, int techId)
    {
        return await _context.UserTechProgress
            .AsNoTracking()
            .AnyAsync(utp => utp.UserId == userId && utp.TechId == techId);
    }

    public async Task<bool> DeleteAsync(int userId, int techId)
    {
        var entity = new UserTechProgress
        {
            UserId = userId,
            TechId = techId
        };

        _context.UserTechProgress.Attach(entity);
        _context.UserTechProgress.Remove(entity);
        return await _context.SaveChangesAsync() > 0;
    }
}