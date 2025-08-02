using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.Projects;

public class DeleteProjectUseCase
{
    private readonly IProjectRepository _repository;

    public DeleteProjectUseCase(IProjectRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExecuteAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
            throw new NotFoundException($"Project with id {id} not found.");

        return await _repository.DeleteAsync(id);
    }
}