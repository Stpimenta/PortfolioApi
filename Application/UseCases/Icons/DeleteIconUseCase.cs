using PortfolioApi.Application.Services;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.Icons;

public class DeleteIconUseCase
{
    private readonly IIconRepository _iconRepository;
    private readonly AmazonS3Service _amazonS3;

    public DeleteIconUseCase(IIconRepository iconRepository,  AmazonS3Service amazonS3)
    {
        _iconRepository = iconRepository;
        _amazonS3 = amazonS3;
    }

    public async Task<bool> ExecuteAsync(int iconId)
    {
        var icon = await _iconRepository.GetIconByIdAsync(iconId);
        if (icon is null)
        {
            throw new NotFoundException($"Icon {iconId} not found");
        }

        if (icon.Path is not null)
            await _amazonS3.DeleteFileAsync(icon.Path);
        
        var isDeleted = await _iconRepository.DeleteIconAsync(iconId);
        return isDeleted;
    }
}