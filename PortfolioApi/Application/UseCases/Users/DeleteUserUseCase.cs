using AutoMapper;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.Users;

public class DeleteUserUseCase
{
    private readonly IUserRepository _userRepository;
    

    public DeleteUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<bool> ExecuteAsync(int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user is null)
        {
            throw new NotFoundException($"user {userId} not found");
        }

        var isDeleted = await _userRepository.DeleteUserAsync(userId);
        return isDeleted;
    }
}