using PortfolioApi.Application.Dtos;
using PortfolioApi.Application.Services;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.Icons;

public class UpdateIconUseCase
{
    private readonly IIconRepository _iconRepository;
    private readonly AmazonS3Service _amazonS3Service;

    public UpdateIconUseCase(IIconRepository iconRepository,  AmazonS3Service amazonS3)
    {
        _iconRepository = iconRepository;
        _amazonS3Service = amazonS3;
        
    }

    public async Task<int> ExecuteAsync(int iconId, UpdateIconDto iconDto)
    {
        var existingIcon = await _iconRepository.GetIconByIdAsync(iconId);
        if (existingIcon == null)
        {
            throw new NotFoundException($"Icon with id {iconId} not found.");
        }
        

        if (iconDto.Icon is not null && iconDto.Icon.Length > 0)
        {
            var extension = Path.GetExtension(iconDto.Icon.FileName).ToLowerInvariant();
            if (extension != ".png" &&  extension != ".jpg" &&  extension != ".jpeg")
                throw new BusinessException($"invalid file extension, only png, jpg or jpeg is allowed {iconDto.Icon.FileName}.");

            string keyName = $"icons/public/{Guid.NewGuid()}{extension}";
            var url = await _amazonS3Service.UploadFile(iconDto.Icon.OpenReadStream(), keyName);

            await _amazonS3Service.DeleteFileAsync(existingIcon.Path!);
            existingIcon.Path = url;
        }
        
        existingIcon.Name = iconDto.Name;
        
        
        await _iconRepository.UpdateIconAsync(existingIcon);
        return iconId;
    }
}