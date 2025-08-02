using Microsoft.EntityFrameworkCore;
using PortfolioApi.Domain.Entities;
using PortfolioApi.Infrastructure.Data;
using PortfolioApi.Infrastructure.Repository.Interfaces;

namespace PortfolioApi.Infrastructure.Repository.Implementations;

public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _context;

    public RoleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Role>> GetAllAsync()
    {
        return await _context.Roles
            .Include(r => r.Icon)
            .Include(r => r.Technologies)
                .ThenInclude(t => t.Icon)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Role?> GetByIdAsync(int id)
    {
        return await _context.Roles
            .Include(r => r.Icon)
            .Include(r => r.Technologies)
                .ThenInclude(t => t.Icon) 
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<int> AddAsync(Role role)
    {
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();
        return role.Id;
    }

    public async Task<int> UpdateAsync(Role role, int roleId)
    {
        _context.Roles.Update(role);
        await _context.SaveChangesAsync();
        return roleId;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null) return false;

        _context.Roles.Remove(role);
        return await _context.SaveChangesAsync() > 0;
    }
}