using AutoMapper;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Infrastructure.Repository.Implementations;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.Projects;

public class UpdateProjectUseCase
{
    private readonly IProjectRepository _repository;
    private readonly ITechnologyRepository _repositoryTechnology;
    private readonly IUserRepository _repositoryUser;
    
    private readonly IMapper _mapper;

    public UpdateProjectUseCase(IProjectRepository repository, IMapper mapper, ITechnologyRepository repositoryTechnology, IUserRepository repositoryUser)
    {
        _repository = repository;
        _mapper = mapper;
        _repositoryTechnology = repositoryTechnology;
        _repositoryUser = repositoryUser;
    }

    public async Task<int> ExecuteAsync(int id, UpdateProjectDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
            throw new NotFoundException($"Project with id {id} not found.");

    
        var techs = await _repositoryTechnology.GetAllAsync();
        var validTechs = techs
            .Where(t => dto.TechnologyIds.Contains(t.Id))
            .ToList();

        var missingTechIds = dto.TechnologyIds.Except(validTechs.Select(t => t.Id)).ToList();
        if (missingTechIds.Any())
            throw new NotFoundException($"Technologies with IDs {string.Join(", ", missingTechIds)} not found.");


        var checkUser = await _repositoryUser.GetUserByIdAsync(dto.UserId);
        if (checkUser == null)
            throw new NotFoundException($"User with ID {dto.UserId} not found.");

  
        _mapper.Map(dto, existing);


        existing.Technologies.Clear();
        foreach (var tech in validTechs)
        {
            existing.Technologies.Add(tech);
        }
        
        await _repository.UpdateAsync(existing);
        return existing.Id;
    }
}