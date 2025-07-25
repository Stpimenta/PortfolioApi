using PortfolioApi.Application.Dtos;
using PortfolioApi.Domain.Entities;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.Users;

public class UpdateUserUseCase
{
    private readonly IUserRepository _userRepository;

    public UpdateUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<int> ExecuteAsync(int userId, UpdateUserDto userDto)
    {
        var existingUser = await _userRepository.GetUserByIdAsync(userId);
        if (existingUser == null)
        {
            throw new NotFoundException($"User with id {userId} not found.");
        }
        
        existingUser.Name = userDto.Name;
        existingUser.Email = userDto.Email;
        
        await _userRepository.UpdateUserAsync(existingUser, userId);
        return userId;
    }
}