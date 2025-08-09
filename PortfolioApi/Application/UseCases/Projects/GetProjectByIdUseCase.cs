using AutoMapper;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.Projects;

public class GetProjectByIdUseCase
{
    private readonly IProjectRepository _repository;
    private readonly IMapper _mapper;
    public GetProjectByIdUseCase(IProjectRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GetProjectDto> ExecuteAsync(int id)
    {
        var project = await _repository.GetByIdAsync(id);
        if (project == null)
            throw new NotFoundException($"Project with id {id} not found.");
        
        
        return _mapper.Map<GetProjectDto>(project);
    }
}