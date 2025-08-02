using PortfolioApi.Application.Dtos;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.Users;

public class UpdateUserUseCase
{
    private readonly IUserRepository _repository;

    public UpdateUserUseCase(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> ExecuteAsync(int id, UpdateUserDto dto)
    {
        var existingUser = await _repository.GetUserByIdAsync(id);
        if (existingUser == null)
        {
            throw new NotFoundException($"User with id {id} not found.");
        }

        existingUser.Name = dto.Name;
        existingUser.Email = dto.Email;
        existingUser.Password = dto.Password;

        await _repository.UpdateUserAsync(existingUser, id);
        return id;
    }
}