using Microsoft.EntityFrameworkCore;
using PortfolioApi.Domain.Entities;
using PortfolioApi.Infrastructure.Data;
using PortfolioApi.Infrastructure.Repository.Interfaces;

namespace PortfolioApi.Infrastructure.Repository.Implementations;

public class UserRoleProgressRepository:IUserRoleProgressRepository
{
    private readonly AppDbContext _context;
    
    public UserRoleProgressRepository( AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserRoleProgress>> getByUserAsync(int userId)
    {
        return await _context.UserRoleProgress
            .Where(urp => urp.UserId == userId)
            .Include(urp => urp.Role)
                .ThenInclude(r => r.Icon)
            .Include(urp => urp.Projects)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<UserRoleProgress?> GetAsync(int userId, int roleId)
    {
        return await _context.UserRoleProgress
            .Include(urp => urp.Role)
            .ThenInclude(r => r.Icon)
            .Include(urp => urp.Projects)
            .FirstOrDefaultAsync(urp => urp.UserId == userId && urp.RoleId == roleId);
    }

    public async Task AddAsync(UserRoleProgress userRoleProgress)
    {
        _context.UserRoleProgress.Add(userRoleProgress);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(UserRoleProgress userRoleProgress)
    {
        _context.UserRoleProgress.Update(userRoleProgress);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int userId, int roleId)
    {
        return await _context.UserRoleProgress
            .AsNoTracking()
            .AnyAsync(urp => urp.UserId == userId && urp.RoleId == roleId);
    }

    public async Task<bool> DeleteAsync(int userId, int roleId)
    {
        var entity = new UserRoleProgress
        {
            UserId = userId,
            RoleId = roleId
        };

        _context.UserRoleProgress.Attach(entity);
        _context.UserRoleProgress.Remove(entity);
        return await _context.SaveChangesAsync() > 0;
        
    }
}