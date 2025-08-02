using PortfolioApi.Domain.Entities;

namespace PortfolioApi.Infrastructure.Repository.Interfaces;

public interface IIconRepository
{
    Task<IEnumerable<Icons>> GetAllAsync();
    Task<Icons?> GetIconByIdAsync(int iconId);
    Task<int> AddIconAsync(Icons icon);
    Task<int> UpdateIconAsync(Icons icon);
    Task<bool> DeleteIconAsync(int iconId);
}