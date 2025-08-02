using Microsoft.EntityFrameworkCore;
using PortfolioApi.Domain.Entities;
using PortfolioApi.Infrastructure.Data;

namespace PortfolioApi.Infrastructure.Repository.Implementations;

public class TechnologyRepository : ITechnologyRepository
{
    private readonly AppDbContext _context;

    public TechnologyRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Technology>> GetAllAsync()
    {
        return await _context.Technologies
            .Include(t => t.Icon) 
            .AsNoTracking()      
            .ToListAsync();
    }

    public async Task<Technology?> GetByIdAsync(int id)
    {
        return await _context.Technologies
            .Include(t => t.Icon) 
            .FirstOrDefaultAsync(t => t.Id == id);
    }
    public async Task<int> AddAsync(Technology tech)
    {
        _context.Technologies.Add(tech);
        await _context.SaveChangesAsync();
        return tech.Id;
    }

    public async Task<int> UpdateAsync(Technology tech, int id)
    {
        _context.Technologies.Update(tech);
        await _context.SaveChangesAsync();
        return id;
    }


    public async Task<bool> DeleteAsync(int id)
    {
        var tech = await _context.Technologies.FindAsync(id);
        if (tech == null) return false;

        _context.Technologies.Remove(tech);
        return await _context.SaveChangesAsync() > 0;
    }
}