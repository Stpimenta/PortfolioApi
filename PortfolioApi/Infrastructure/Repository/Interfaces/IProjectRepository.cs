using PortfolioApi.Domain.Entities;

namespace PortfolioApi.Infrastructure.Repository.Interfaces;

public interface IProjectRepository
{
    Task<IEnumerable<Project>> GetAllAsync();
    Task<Project?> GetByIdAsync(int projectId);
    Task<int> AddAsync(Project project);
    Task UpdateAsync(Project project);
    Task<bool> DeleteAsync(int projectId);
    Task<IEnumerable<Project>> GetByUserIdAsync(int userId);
    Task<List<Project>> GetProjectsByUserIdAndProjectIdsAsync(int userId, List<int> projectIds);
}