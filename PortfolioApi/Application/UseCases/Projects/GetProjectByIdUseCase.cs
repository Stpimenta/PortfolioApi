using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.Projects;

public class GetProjectByIdUseCase
{
    private readonly IProjectRepository _repository;

    public GetProjectByIdUseCase(IProjectRepository repository)
    {
        _repository = repository;
    }

    public async Task<Domain.Entities.Project> ExecuteAsync(int id)
    {
        var project = await _repository.GetByIdAsync(id);
        if (project == null)
            throw new NotFoundException($"Project with id {id} not found.");

        return project;
    }
}