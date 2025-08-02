using PortfolioApi.Application.Dtos;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.Icons;

public class UpdateIconUseCase
{
    private readonly IIconRepository _iconRepository;

    public UpdateIconUseCase(IIconRepository iconRepository)
    {
        _iconRepository = iconRepository;
    }

    public async Task<int> ExecuteAsync(int iconId, UpdateIconDto iconDto)
    {
        var existingIcon = await _iconRepository.GetIconByIdAsync(iconId);
        if (existingIcon == null)
        {
            throw new NotFoundException($"Icon with id {iconId} not found.");
        }
        
        existingIcon.Name = iconDto.Name;
        existingIcon.Path = iconDto.Path;
        
        await _iconRepository.UpdateIconAsync(existingIcon);
        return iconId;
    }
}