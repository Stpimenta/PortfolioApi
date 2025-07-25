using PortfolioApi.Domain.Entities;

namespace PortfolioApi.Infrastructure.Repository.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetUserByIdAsync(int userId);
    Task <int> AddUserAsync(User user);
    
    Task<bool> UpdateUserAsync(User user, int userId);
    
    Task<bool> DeleteUserAsync(int userId);
    
    Task<User?> GetUserByEmail(string email);
}