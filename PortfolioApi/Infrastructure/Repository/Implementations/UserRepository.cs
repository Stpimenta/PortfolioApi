using Microsoft.EntityFrameworkCore;
using PortfolioApi.Domain.Entities;
using PortfolioApi.Infrastructure.Data;
using PortfolioApi.Infrastructure.Repository.Interfaces;

namespace PortfolioApi.Infrastructure.Repository.Implementations;

public class UserRepository: IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        
        return await _context.Users
            .Include(u => u.Projects)
            .FirstOrDefaultAsync(u => u.Id == userId);
            
    }

    public async Task<int> AddUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user.Id;
    }

    public async Task<bool> UpdateUserAsync(User user, int userId)
    {
        var existing = await _context.Users.FindAsync(userId);
        if (existing == null) return false;

        existing.Name = user.Name;
        existing.Email = user.Email;

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteUserAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return false;

        _context.Users.Remove(user);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}