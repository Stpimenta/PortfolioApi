using PortfolioApi.Domain.Entities;

namespace PortfolioApi.Infrastructure.Repository.Interfaces;

public interface IRoleRepository
{
    Task<IEnumerable<Role>> GetAllAsync();
    Task<Role?> GetByIdAsync(int id);
    Task<int> AddAsync(Role role);
    Task<int> UpdateAsync(Role role, int id);
    Task<bool> DeleteAsync(int id);
}