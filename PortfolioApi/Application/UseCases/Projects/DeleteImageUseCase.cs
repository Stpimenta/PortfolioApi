using PortfolioApi.Application.Services;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.Projects;

public class DeleteImageUseCase
{
    private readonly IProjectRepository _projectRepository;
    private readonly AmazonS3Service _amazonS3Service;

    public DeleteImageUseCase(IProjectRepository projectRepository, AmazonS3Service amazonS3Service)
    {
        _projectRepository = projectRepository;
        _amazonS3Service = amazonS3Service;
    }

    public async Task ExecuteAsync(int  projectId, string imageUrl)
    {
        var project  = await _projectRepository.GetByIdAsync(projectId);

        if (project is null)
            throw new NotFoundException($"project with id: {projectId} not found");

        var decodedImageUrl = Uri.UnescapeDataString(imageUrl);
        
        if (project.Images == null || !project.Images.Remove(decodedImageUrl))
            throw new NotFoundException("Image not found in project.");
        
        await _amazonS3Service.DeleteFileAsync(decodedImageUrl);
        await _projectRepository.UpdateAsync(project);

    }
    
}