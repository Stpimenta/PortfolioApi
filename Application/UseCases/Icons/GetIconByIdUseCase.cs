using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.Icons;

public class GetIconByIdUseCase
{
    private readonly IIconRepository _iconRepository;

    public GetIconByIdUseCase(IIconRepository iconRepository)
    {
        _iconRepository = iconRepository;
    }

    public async Task<Domain.Entities.Icons> ExecuteAsync(int iconId)
    {
        var icon = await _iconRepository.GetIconByIdAsync(iconId);
        if (icon == null)
            throw new NotFoundException($"Icon with id {iconId} not found.");
        return icon;
    }
}