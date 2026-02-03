using PortfolioApi.Application.Dtos;
using PortfolioApi.Infrastructure.Repository.Implementations;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

public class UpdateRoleUseCase
{
    private readonly IRoleRepository _repository;
    private readonly ITechnologyRepository _repositoryTechnology;
    private readonly IIconRepository _repositoryIcon;
    
    public UpdateRoleUseCase(IRoleRepository repository,  ITechnologyRepository repositoryTechnology, IIconRepository repositoryIcon)
    {
        _repository = repository;
        _repositoryTechnology = repositoryTechnology;
        _repositoryIcon = repositoryIcon;
        
    }

    public async Task<int> ExecuteAsync(int id, UpdateRoleDto dto)
    {
        var existingRole = await _repository.GetByIdAsync(id);
        if (existingRole == null)
            throw new NotFoundException($"Role with id {id} not found.");
        
       

        if (dto.IconId.HasValue)
        {
            var existIcon = await _repositoryIcon.GetIconByIdAsync(dto.IconId.Value);

            if (existIcon == null)
            {
                throw new NotFoundException($"Icon with ID  {dto.IconId.Value} not found");
            }
            existingRole.IconId = dto.IconId.Value;
        }
        else
        {
            existingRole.IconId = null;
        }

   
        existingRole.Name = dto.Name;


        var techs = await _repositoryTechnology.GetAllAsync();
        var validTechs = techs.Where(t => dto.TechnologyIds.Contains(t.Id)).ToList();

        var missingTechIds = dto.TechnologyIds.Except(validTechs.Select(t => t.Id)).ToList();
        if (missingTechIds.Any())
            throw new NotFoundException($"Technologies with IDs {string.Join(", ", missingTechIds)} not found.");

        existingRole.Technologies.Clear();
        foreach (var tech in validTechs)
            existingRole.Technologies.Add(tech);

        await _repository.UpdateAsync(existingRole, id);
        return id;
    }
}