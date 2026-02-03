using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.Roles;


public class DeleteRoleUseCase
{
    private readonly IRoleRepository _repository;

    public DeleteRoleUseCase(IRoleRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExecuteAsync(int id)
    {
        var existingRole = await _repository.GetByIdAsync(id);
        if (existingRole == null)
        {
            throw new NotFoundException($"Role with id {id} not found.");
        }

        return await _repository.DeleteAsync(id);
    }
}