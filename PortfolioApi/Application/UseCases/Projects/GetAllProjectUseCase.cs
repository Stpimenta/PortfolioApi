using AutoMapper;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Infrastructure.Repository.Interfaces;

namespace PortfolioApi.Application.UseCases.Projects;

public class GetAllProjectsUseCase
{
    private readonly IProjectRepository _repository;
    private  readonly IMapper _mapper;
    public GetAllProjectsUseCase(IProjectRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetProjectDto>> ExecuteAsync()
    {
        var  projects = await _repository.GetAllAsync();
        var projectDtos = _mapper.Map<IEnumerable<GetProjectDto>>(projects);
        return projectDtos;
    }
}