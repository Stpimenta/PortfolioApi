using PortfolioApi.Domain.Entities;

namespace PortfolioApi.Infrastructure.Repository.Interfaces;

public interface IProjectRepository
{
    Task<IEnumerable<Project>> GetAllAsync();
    Task<Project?> GetByIdAsync(int projectId);
    Task<int> AddAsync(Project project);
    Task<int> UpdateAsync(Project project);
    Task<bool> DeleteAsync(int projectId);
}