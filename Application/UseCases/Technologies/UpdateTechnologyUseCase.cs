using PortfolioApi.Application.Dtos;
using PortfolioApi.Infrastructure.Repository.Implementations;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.Technologies;

public class UpdateTechnologyUseCase
{
    private readonly ITechnologyRepository _repository;
    private readonly IIconRepository _iconRepository;

    public UpdateTechnologyUseCase(ITechnologyRepository repository,  IIconRepository iconRepository)
    {
        _repository = repository;
        _iconRepository = iconRepository;
    }

    public async Task<int> ExecuteAsync(int id, UpdateTechnologyDto dto)
    {
        var existingTech = await _repository.GetByIdAsync(id);
        if (existingTech == null)
        {
            throw new NotFoundException($"Technology with id {id} not found.");
        }

        if (dto.IconId.HasValue)
        {
            var existingIcon = await _iconRepository.GetIconByIdAsync(id);
            if (existingIcon == null)
            {
                throw new NotFoundException($"Icon with id {id} not found.");
            }

        }
       
        existingTech.Name = dto.Name;
        existingTech.IconId = dto.IconId;

        await _repository.UpdateAsync(existingTech, id);
        return id;
    }
}