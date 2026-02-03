
using AutoMapper;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Application.Services;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.Users;

public class LoginUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly JwtService _jwtService;

    public LoginUserUseCase(IUserRepository userRepository, IPasswordService passwordService, JwtService jwtService)
    {
        _userRepository = userRepository;
        _passwordService  = passwordService;
        _jwtService = jwtService;
    }

    public async Task<LoginResponseDto> ExecuteAsync(string email, string password)
    {
        var user = await _userRepository.GetUserByEmail(email);

        if (user is not null && _passwordService.VerifyPassword (user.Password!,password))
        {
            return new LoginResponseDto()
            {
                Token = _jwtService.GenerateToken(user.Id),
                ExpiresAt = DateTime.Now.AddHours(2)
            };
        }

        throw new AuthenticationException("Invalid credentials.");
    }
}