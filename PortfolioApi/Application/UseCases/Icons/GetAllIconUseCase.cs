using PortfolioApi.Infrastructure.Repository.Interfaces;

namespace PortfolioApi.Application.UseCases.Icons;

public class GetAllIconsUseCase
{
    private readonly IIconRepository _iconRepository;

    public GetAllIconsUseCase(IIconRepository iconRepository)
    {
        _iconRepository = iconRepository;
    }

    public async Task<IEnumerable<Domain.Entities.Icons>> ExecuteAsync()
    {
        return await _iconRepository.GetAllAsync();
    }
}