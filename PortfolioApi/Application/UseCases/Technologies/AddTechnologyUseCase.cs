using AutoMapper;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Infrastructure.Repository.Implementations;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.Technologies;

public class AddTechnologyUseCase
{
    private readonly ITechnologyRepository _repository;
    private readonly IMapper _mapper;
    private readonly IIconRepository _iconRepository;
    
    public AddTechnologyUseCase(ITechnologyRepository repository , IMapper mapper,  IIconRepository iconRepository)
    {
        _repository = repository;
        _mapper = mapper;
        _iconRepository = iconRepository;
    }

    public async Task<int> ExecuteAsync(CreateTechnologyDto dto)
    {
        if (dto.IconId.HasValue)
        {
            var iconExists = await _iconRepository.GetIconByIdAsync(dto.IconId.Value);
            if (iconExists == null)
                throw new NotFoundException($"Icon with id {dto.IconId.Value} not found.");
        }
        
        var technology = _mapper.Map<Domain.Entities.Technology>(dto);
        return await _repository.AddAsync(technology);
    }
}