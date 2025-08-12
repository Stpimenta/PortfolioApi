using PortfolioApi.Application.Services;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.Projects;

public class AddImageUseCase
{
    private readonly IProjectRepository _projectRepository;
    private readonly AmazonS3Service _amazonS3Service;

    public AddImageUseCase(IProjectRepository projectRepository, AmazonS3Service amazonS3Service)
    {
        _projectRepository = projectRepository;
        _amazonS3Service = amazonS3Service;
    }

    public  async Task ExecuteAsync(int projectId, List<IFormFile> images)
    {
        if (!images.Any())
        {
            throw new BusinessException("image list is empty");
        }

       var project = await  _projectRepository.GetByIdAsync(projectId);
       
       if(project is null)
           throw new NotFoundException($"project with id: {projectId} not found");


       project.Images ??= new List<string>();
           
       foreach (var image in images)
       {
           var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
           if(extension != ".png" &&  extension != ".jpg" &&  extension != ".jpeg")
               throw new BusinessException($"invalid file extension, only png, jpg or jpeg is allowed {image.FileName}.");

           string keyName = $"project_images/{Guid.NewGuid()}{extension}";
           var url = await  _amazonS3Service.UploadFile(image.OpenReadStream(), keyName);
           
           project.Images.Add(url);
           
       }

       await _projectRepository.UpdateAsync(project);
    }
    
}