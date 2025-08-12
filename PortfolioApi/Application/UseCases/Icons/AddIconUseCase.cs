using AutoMapper;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Application.Services;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Domain.Entities;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.Icons;

public class AddIconUseCase
{
    private readonly IIconRepository _iconRepository;
    private readonly IMapper _mapper;
    private readonly AmazonS3Service _amazonS3Service;

    public AddIconUseCase(IIconRepository iconRepository, IMapper mapper,  AmazonS3Service amazonS3Service)
    {
        _iconRepository = iconRepository;
        _mapper = mapper;
        _amazonS3Service = amazonS3Service;
    }

    public async Task<int> ExecuteAsync(CreateIconDto iconDto)
    {
        if (iconDto.Icon== null || iconDto.Icon.Length == 0)
            throw new BusinessException("file is empty");
        
        var extension = Path.GetExtension(iconDto.Icon.FileName).ToLowerInvariant();
        if (extension != ".png" &&  extension != ".jpg" &&  extension != ".jpeg")
            throw new BusinessException($"invalid file extension, only png, jpg or jpeg is allowed {iconDto.Icon.FileName}.");

        string keyName = $"icons/public/{Guid.NewGuid()}{extension}";
        var url = await _amazonS3Service.UploadFile(iconDto.Icon.OpenReadStream(), keyName);
        
        var entityIcon = _mapper.Map<Domain.Entities.Icons>(iconDto);
        entityIcon.Path = url;
        
        return await _iconRepository.AddIconAsync(entityIcon);
    }
}