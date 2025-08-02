using PortfolioApi.Domain.Entities;

namespace PortfolioApi.Infrastructure.Repository.Implementations;

public interface ITechnologyRepository
{
    Task<IEnumerable<Technology>> GetAllAsync();
    Task<Technology?> GetByIdAsync(int id);
    Task<int> AddAsync(Technology tech);
    Task<int> UpdateAsync(Technology tech, int id);
    Task<bool> DeleteAsync(int id);
}