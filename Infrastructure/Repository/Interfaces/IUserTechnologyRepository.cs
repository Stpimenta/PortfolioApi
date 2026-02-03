using PortfolioApi.Domain.Entities;

namespace PortfolioApi.Infrastructure.Repository.Interfaces;

public interface IUserTechnologyRepository
{
    Task<IEnumerable<UserTechProgress>> GetByUserAsync(int userId);
    Task<UserTechProgress?> GetAsync(int userId, int techId);
    Task AddAsync(UserTechProgress userTechProgress);
    Task UpdateAsync(UserTechProgress userTechProgress);
    Task<bool> ExistsAsync(int userId, int techId);
    Task<bool> DeleteAsync(int userId, int techId);
}