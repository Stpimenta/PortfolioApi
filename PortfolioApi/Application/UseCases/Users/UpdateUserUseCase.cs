using PortfolioApi.Application.Dtos;
using PortfolioApi.Application.Services;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.Users;

public class UpdateUserUseCase
{

    private readonly IUserRepository _repository;
    private readonly AmazonS3Service  _amazonS3Service;

    public UpdateUserUseCase(IUserRepository repository,  AmazonS3Service  amazonS3Service)
    {
        _repository = repository;
        _amazonS3Service = amazonS3Service;
    }

    public async Task<int> ExecuteAsync(int id, UpdateUserDto userDto)
    {
        var existingUser = await _repository.GetUserByIdAsync(id);
        if (existingUser == null)
        {
            throw new NotFoundException($"User with id {id} not found.");
        }

        existingUser.Name = userDto.Name;
        existingUser.Email = userDto.Email;
        existingUser.Password = userDto.Password;
        
        if (userDto.ConfigUrl is not null && userDto.ConfigUrl.Length > 0)
        {
            if (existingUser.ConfigUrl is not null && !string.IsNullOrEmpty(existingUser.ConfigUrl))
            {
                await _amazonS3Service.DeleteFileAsync(existingUser.ConfigUrl);
            }
            
            var extension = Path.GetExtension(userDto.ConfigUrl.FileName).ToLowerInvariant();
            if(extension != ".json")
                throw new BusinessException("invalid file extension, only png is allowed.");
            string keyName = $"user_config/{Guid.NewGuid()}{extension}";
            var url = await _amazonS3Service.UploadFile(userDto.ConfigUrl.OpenReadStream(), keyName);
            existingUser.ConfigUrl = url;
        }

        await _repository.UpdateUserAsync(existingUser, id);
        return id;
    }
}