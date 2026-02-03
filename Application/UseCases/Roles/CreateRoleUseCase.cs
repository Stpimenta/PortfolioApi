using AutoMapper;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Domain.Entities;
using PortfolioApi.Infrastructure.Repository.Implementations;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.Roles;

public class CreateRoleUseCase
{
    private readonly IRoleRepository _repository;
    private readonly IMapper _mapper;
    private readonly ITechnologyRepository _repositoryTechnology;
    private readonly IIconRepository _repositoryIcon;

    public CreateRoleUseCase(IRoleRepository repository, IMapper mapper,  ITechnologyRepository repositoryTechnology, IIconRepository repositoryIcon)
    {
        _repository = repository;
        _mapper = mapper;
        _repositoryTechnology = repositoryTechnology;
        _repositoryIcon = repositoryIcon;
    }

    public async Task<int> ExecuteAsync(CreateRoleDto dto)
    {
        if (dto.IconId.HasValue)
        {
            var existIcon = await _repositoryIcon.GetIconByIdAsync(dto.IconId.Value);

            if (existIcon == null)
            {
                throw new NotFoundException($"Icon with ID  {dto.IconId.Value} not found");
            }
        }
        
        var techs = await _repositoryTechnology.GetAllAsync();

        var validTechs = techs
            .Where(t => dto.TechnologyIds.Contains(t.Id))
            .ToList();

        var missingTechIds = dto.TechnologyIds
            .Except(validTechs.Select(t => t.Id))
            .ToList();

        if (missingTechIds.Any())
            throw new NotFoundException($"Technologies with IDs {string.Join(", ", missingTechIds)} not found.");

        var role = _mapper.Map<Role>(dto);
        role.Technologies = validTechs;

        return await _repository.AddAsync(role);
    }
}