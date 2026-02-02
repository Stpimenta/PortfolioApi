using Amazon.S3;
using AutoMapper;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Application.Services;
using PortfolioApi.Infrastructure.Repository.Implementations;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.Projects;

public class UpdateProjectUseCase
{
    private readonly IProjectRepository _repository;
    private readonly ITechnologyRepository _repositoryTechnology;
    private readonly IUserRepository _repositoryUser;
    private readonly AmazonS3Service _amazonS3Service;
    private readonly IMapper _mapper;

    public UpdateProjectUseCase(IProjectRepository repository, IMapper mapper,
        ITechnologyRepository repositoryTechnology, IUserRepository repositoryUser,  AmazonS3Service amazonS3Servicet)
    {
        _repository = repository;
        _mapper = mapper;
        _repositoryTechnology = repositoryTechnology;
        _repositoryUser = repositoryUser;
        _amazonS3Service = amazonS3Servicet;
        
    }

    public async Task ExecuteAsync(int id, UpdateProjectDto dto)
    {
        var project = await _repository.GetByIdAsync(id);
        if (project is null)
            throw new NotFoundException($"project with id:{id} not found");
        
        var user = await _repositoryUser.GetUserByIdAsync(dto.UserId);
        if (user is null)
            throw new NotFoundException($"User with id:{dto.UserId} not found");
        
        _mapper.Map(dto, project);
        
        if (dto.TechnologyIds == null || !dto.TechnologyIds.Any())
        {
        
            project.Technologies.Clear();
        }
        else
        {
            var technologies = await _repositoryTechnology.GetByTechIdsAsync(dto.TechnologyIds);

            if (dto.TechnologyIds.Count != technologies.Count())
            {
                var notFoudIds = dto.TechnologyIds
                    .Except(technologies.Select(t => t.Id))
                    .ToList();
                throw new NotFoundException($"technologies with Ids {string.Join(", ", notFoudIds)} not found");
            }

            var toRemove = project.Technologies.Where(t => !dto.TechnologyIds.Contains(t.Id)).ToList();
            foreach (var tech in toRemove)
            {
                project.Technologies.Remove(tech);
            }

            var toAdd = technologies.Where(t => project.Technologies.All(pt => pt.Id != t.Id)).ToList();
            foreach (var tech in toAdd)
            {
                project.Technologies.Add(tech);
            }
        }

        if (dto.Icon is not null && dto.Icon.Length > 0)
        {
            var extension = Path.GetExtension(dto.Icon.FileName).ToLowerInvariant();
            if (extension != ".png" &&  extension != ".jpg" &&  extension != ".jpeg")
                throw new BusinessException($"invalid file extension, only png, jpg or jpeg is allowed {dto.Icon.FileName}.");
            
            string keyName = $"icons/project/{Guid.NewGuid()}{extension}";
            var iconId = await _amazonS3Service.UploadFile(dto.Icon.OpenReadStream(), keyName);

            if (project.Icon is not null)
            {
                await _amazonS3Service.DeleteFileAsync(project.Icon);
            }
            project.Icon = iconId;
        }
        
        if (dto.Config != null && dto.Config.Length > 0)
        {
            var extension = Path.GetExtension(dto.Config.FileName).ToLowerInvariant();
            if (extension != ".json")
                throw new BusinessException($"invalid file extension, only json is allowed {dto.Config.FileName}.");
            string keyName = $"project_config/{Guid.NewGuid()}{extension}";
            var url = await _amazonS3Service.UploadFile(dto.Config.OpenReadStream(), keyName);
            await _amazonS3Service.DeleteFileAsync(project.Config);
            project.Config = url;
        }
       
        if(!string.IsNullOrEmpty(project.Description))
            project.Description = project.Description.Replace("\\n", "\n");
        
        await _repository.UpdateAsync(project);
    }

}