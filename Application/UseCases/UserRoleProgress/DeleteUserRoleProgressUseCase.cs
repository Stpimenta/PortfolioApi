using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.UserRoleProgress;

public class DeleteUserRoleProgressUseCase
{
    private readonly IUserRoleProgressRepository _repository;

    public DeleteUserRoleProgressUseCase(IUserRoleProgressRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int userId, int roleId)
    {
        var exists = await _repository.ExistsAsync(userId, roleId);
        if (!exists)
            throw new BusinessException("User role progress not found for deletion.");

        await _repository.DeleteAsync(userId, roleId);
    }
}