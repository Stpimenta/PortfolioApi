using AutoMapper;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Application.Services;
using PortfolioApi.Domain.Entities;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.Users;

public class AddUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IPasswordService _passwordService;

    public AddUserUseCase(IUserRepository userRepository, IMapper mapper, IPasswordService passwordService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _passwordService = passwordService;
    }
 
    public async Task<int> ExecuteAsync(CreateUserDto userDto)
    {
        var emailAlreadyExist = await _userRepository.GetUserByEmail(userDto.Email);
        if ( emailAlreadyExist is not null)
        {
            throw new BusinessException("email already registered.");
        }
        
        var hashPass = _passwordService.HashPassword(userDto.Password);

        var user = _mapper.Map<User>(userDto);
        user.Password = hashPass;  

        return await _userRepository.AddUserAsync(user);
    }
}
