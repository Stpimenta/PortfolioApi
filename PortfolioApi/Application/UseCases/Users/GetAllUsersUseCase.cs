using PortfolioApi.Domain.Entities;
using PortfolioApi.Infrastructure.Repository.Interfaces;

namespace PortfolioApi.Application.UseCases.Users;

public class GetAllUsersUseCase
{
    private readonly IUserRepository _repository;

    public GetAllUsersUseCase(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<User>> ExecuteAsync()
    {
        return await _repository.GetAllAsync();
    }


}