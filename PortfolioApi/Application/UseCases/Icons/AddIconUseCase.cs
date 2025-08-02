using AutoMapper;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Domain.Entities;

namespace PortfolioApi.Application.UseCases.Icons;

public class AddIconUseCase
{
    private readonly IIconRepository _iconRepository;
    private readonly IMapper _mapper;

    public AddIconUseCase(IIconRepository iconRepository, IMapper mapper)
    {
        _iconRepository = iconRepository;
        _mapper = mapper;
    }

    public async Task<int> ExecuteAsync(CreateIconDto iconDto)
    {
        var icon = _mapper.Map<Domain.Entities.Icons>(iconDto);
        return await _iconRepository.AddIconAsync(icon);
    }
}