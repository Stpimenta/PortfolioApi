using AutoMapper;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Infrastructure.Repository.Implementations;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.Technologies;

public class GetTechnologyByIdUseCase
{
    private readonly ITechnologyRepository _repository;
    private readonly IMapper _mapper;
    public GetTechnologyByIdUseCase(ITechnologyRepository repository,  IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GetTechnologyDto> ExecuteAsync(int id)
    {
        var tech = await _repository.GetByIdAsync(id);
        if (tech == null)
            throw new NotFoundException($"Technology with id {id} not found.");
        return _mapper.Map<GetTechnologyDto>(tech);
    }
}