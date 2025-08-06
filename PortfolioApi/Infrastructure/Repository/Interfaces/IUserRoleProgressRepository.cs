using PortfolioApi.Domain.Entities;

namespace PortfolioApi.Infrastructure.Repository.Interfaces;

public interface IUserRoleProgressRepository
{
  Task<IEnumerable<UserRoleProgress>> getByUserAsync(int userId);
  Task<UserRoleProgress> getAsync(int userId, int roleId);
  Task AddAsync(UserRoleProgress userRoleProgress);
  Task UpdateAsync(UserRoleProgress userRoleProgress);
  Task<bool> DeleteAsync(int userId, int roleId);
  Task<bool> ExistsAsync(int userId, int roleId);
  
}