using AutoMapper;
using PortfolioApi.Application.Dtos;
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

    public AddProjectUseCase(IProjectRepository repository, IMapper mapper, ITechnologyRepository repositoryTechnology, IUserRepository repositoryUser)
    {
        _repository = repository;
        _mapper = mapper;
        _repositoryTechnology = repositoryTechnology;
        _repositoryUser = repositoryUser;
    }

    public async Task<int> ExecuteAsync(CreateProjectDto dto)
    {
        var techs = await _repositoryTechnology.GetAllAsync();

        if (dto.TechnologyIds.Count > 0)
        {
            var validTechs = techs.Where(t => dto.TechnologyIds.Contains(t.Id)).ToList();
        
            var missingTechIds = dto.TechnologyIds.Except(validTechs.Select(t => t.Id)).ToList();
        
            if (missingTechIds.Any())
            {
                throw new NotFoundException($"Technologies with IDs {string.Join(", ", missingTechIds)} not found.");
            }

        }

        var checkUser = await _repositoryUser.GetUserByIdAsync(dto.UserId);
        if (checkUser == null)
            throw new NotFoundException($" User with ID {dto.UserId} not found.");
        
        var project = _mapper.Map<Domain.Entities.Project>(dto);
        return await _repository.AddAsync(project);
    }
}