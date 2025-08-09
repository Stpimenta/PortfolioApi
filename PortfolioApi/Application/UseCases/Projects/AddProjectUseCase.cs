using AutoMapper;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Application.Services;
using PortfolioApi.Domain.Entities;
using PortfolioApi.Infrastructure.Repository.Implementations;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.Projects;

public class AddProjectUseCase
{
    private readonly IProjectRepository _repository;
    private readonly ITechnologyRepository _repositoryTechnology;
    private readonly IUserRepository _repositoryUser;
    private readonly IMapper _mapper;
    private readonly AmazonS3Service _amazonS3;

    public AddProjectUseCase(IProjectRepository repository, IMapper mapper, ITechnologyRepository repositoryTechnology, 
            IUserRepository repositoryUser, AmazonS3Service amazonS3)
    {
        _repository = repository;
        _mapper = mapper;
        _repositoryTechnology = repositoryTechnology;
        _repositoryUser = repositoryUser;
        _amazonS3 = amazonS3;
    }

    public async Task<int> ExecuteAsync(CreateProjectDto dto)
     {

         var user = await _repositoryUser.GetUserByIdAsync(dto.UserId);
         if (user == null)
             throw new NotFoundException($"User with ID {dto.UserId} not found.");

         var allTechnologies = await _repositoryTechnology.GetAllAsync();

         List<Technology> validTechnologies = new();

         if (dto.TechnologyIds?.Count > 0)
         {
             validTechnologies = allTechnologies
                 .Where(t => dto.TechnologyIds.Contains(t.Id))
                 .ToList();

             var missingIds = dto.TechnologyIds.Except(validTechnologies.Select(t => t.Id)).ToList();

             if (missingIds.Any())
                 throw new NotFoundException($"Technologies with IDs {string.Join(", ", missingIds)} not found.");
         }

         
         var project = _mapper.Map<Project>(dto);
         project.Technologies = validTechnologies;
         
         
         if (dto.Icon != null && dto.Icon.Length > 0)
         {
             var extension = Path.GetExtension(dto.Icon.FileName).ToLowerInvariant();
             if (extension != ".png")
                 throw new BusinessException("invalid file extension, only png is allowed.");
             string keyName = $"icons/project/{Guid.NewGuid()}{extension}";
             var iconId = await _amazonS3.UploadFile(dto.Icon.OpenReadStream(), keyName);
             project.Icon = iconId;
         }
         

        
         return await _repository.AddAsync(project);
     }
 }