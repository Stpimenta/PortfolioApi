using Microsoft.EntityFrameworkCore;
using PortfolioApi.Domain.Entities;
using PortfolioApi.Infrastructure.Data;
using PortfolioApi.Infrastructure.Repository.Interfaces;

namespace PortfolioApi.Infrastructure.Repository.Implementations;

public class IconRepository : IIconRepository
{
    private readonly AppDbContext _context;

    public IconRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Icons>> GetAllAsync()
    {
        return await _context.Icons.ToListAsync();
    }

    public async Task<Icons?> GetIconByIdAsync(int iconId)
    {
        return await _context.Icons.FindAsync(iconId);
    }

    public async Task<int> AddIconAsync(Icons icon)
    {
        _context.Icons.Add(icon);
        await _context.SaveChangesAsync();
        return icon.Id;
    }

    public async Task<int> UpdateIconAsync(Icons icon)
    {
        _context.Icons.Update(icon);
        await _context.SaveChangesAsync();
        return icon.Id;
    }

    public async Task<bool> DeleteIconAsync(int iconId)
    {
        var icon = await _context.Icons.FindAsync(iconId);
        if (icon == null)
            return false;

        _context.Icons.Remove(icon);
        return await _context.SaveChangesAsync() > 0;
    }
}