using PortfolioApi.Infrastructure.Repository.Implementations;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.Technologies;

public class DeleteTechnologyUseCase
{
    private readonly ITechnologyRepository _repository;

    public DeleteTechnologyUseCase(ITechnologyRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExecuteAsync(int id)
    {
        var existingTech = await _repository.GetByIdAsync(id);
        if (existingTech == null)
        {
            throw new NotFoundException($"Technology with id {id} not found.");
        }

        return await _repository.DeleteAsync(id);
    }
}
