
using PortfolioApi.Application.Services;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.Users;

public class DeleteUserUseCase
{
    private readonly IUserRepository _repository;
    private readonly AmazonS3Service _amazonS3Service;

    public DeleteUserUseCase(IUserRepository repository,  AmazonS3Service amazonS3Service)
    {
        _repository = repository;
        _amazonS3Service = amazonS3Service;
    }

    public async Task<bool> ExecuteAsync(int id)
    {
        var existingUser = await _repository.GetUserByIdAsync(id);
        if (existingUser == null)
            throw new NotFoundException($"User with id {id} not found.");

        if (existingUser.Config is not null)
        {
            await _amazonS3Service.DeleteFileAsync(existingUser.Config);
        }
        

        return await _repository.DeleteUserAsync(id);
    }
}