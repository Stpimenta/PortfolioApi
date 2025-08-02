using PortfolioApi.Infrastructure.Repository.Interfaces;

namespace PortfolioApi.Application.UseCases.Projects;

public class GetAllProjectsUseCase
{
    private readonly IProjectRepository _repository;

    public GetAllProjectsUseCase(IProjectRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Domain.Entities.Project>> ExecuteAsync()
    {
        return await _repository.GetAllAsync();
    }
}