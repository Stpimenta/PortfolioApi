using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.Users;

public class DeleteUserUseCase
{
    private readonly IUserRepository _repository;

    public DeleteUserUseCase(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExecuteAsync(int id)
    {
        var existingUser = await _repository.GetUserByIdAsync(id);
        if (existingUser == null)
            throw new NotFoundException($"User with id {id} not found.");

        return await _repository.DeleteUserAsync(id);
    }
}