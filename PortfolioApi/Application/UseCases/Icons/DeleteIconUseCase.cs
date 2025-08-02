using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.Icons;

public class DeleteIconUseCase
{
    private readonly IIconRepository _iconRepository;

    public DeleteIconUseCase(IIconRepository iconRepository)
    {
        _iconRepository = iconRepository;
    }

    public async Task<bool> ExecuteAsync(int iconId)
    {
        var icon = await _iconRepository.GetIconByIdAsync(iconId);
        if (icon is null)
        {
            throw new NotFoundException($"Icon {iconId} not found");
        }

        var isDeleted = await _iconRepository.DeleteIconAsync(iconId);
        return isDeleted;
    }
}