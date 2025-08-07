using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.UserTechnologyProgress;

public class DeleteUserTechnologyUseCase
{
    private readonly IUserTechnologyRepository _repository;

    public DeleteUserTechnologyUseCase(IUserTechnologyRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int userId, int techId)
    {
        var exists = await _repository.ExistsAsync(userId, techId);
        if (!exists)
            throw new BusinessException("User technology progress not found for deletion.");

        await _repository.DeleteAsync(userId, techId);
    }
}

