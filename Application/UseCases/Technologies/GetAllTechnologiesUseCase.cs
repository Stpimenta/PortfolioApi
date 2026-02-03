using AutoMapper;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Infrastructure.Repository.Implementations;

namespace PortfolioApi.Application.UseCases.Technologies;

public class GetAllTechnologiesUseCase
{
    private readonly ITechnologyRepository _repository;
    private readonly IMapper _mapper;

    public GetAllTechnologiesUseCase(ITechnologyRepository repository,  IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetTechnologyDto>> ExecuteAsync()
    {
        return _mapper.Map<IEnumerable<GetTechnologyDto>>(await _repository.GetAllAsync());
    }
}