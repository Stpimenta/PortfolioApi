using PortfolioApi.Application.Services;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.Projects;

public class DeleteProjectUseCase
{
    private readonly IProjectRepository _repository;
    private readonly AmazonS3Service _amazonS3;

    public DeleteProjectUseCase(IProjectRepository repository,  AmazonS3Service amazonS3)
    {
        _repository = repository;
        _amazonS3 = amazonS3;
    }

    public async Task<bool> ExecuteAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
            throw new NotFoundException($"Project with id {id} not found.");

        if (existing.Icon is not null)
            await _amazonS3.DeleteFileAsync(existing.Icon);
        
        if (existing.ConfigUrl is not null)
        {
            await _amazonS3.DeleteFileAsync(existing.ConfigUrl);
        }

        if ( existing.Images is not null && existing.Images.Any())
        {
            foreach (var image in existing.Images)
            {
                await _amazonS3.DeleteFileAsync(image);
            }
        }
        
        return await _repository.DeleteAsync(id);
    }
}