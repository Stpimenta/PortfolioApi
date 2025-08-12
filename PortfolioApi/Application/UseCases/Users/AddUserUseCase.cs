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
    private readonly AmazonS3Service _amazonS3Service;

    public AddUserUseCase(IUserRepository userRepository, IMapper mapper, IPasswordService passwordService,  AmazonS3Service amazonS3Service)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _passwordService = passwordService;
        _amazonS3Service = amazonS3Service;
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
        
        
        if (userDto.ConfigUrl is not null && userDto.ConfigUrl.Length > 0)
        {
            var extension = Path.GetExtension(userDto.ConfigUrl.FileName).ToLowerInvariant();
            if(extension != ".json")
                throw new BusinessException("invalid file extension, only png is allowed.");
            string keyName = $"user_config/{Guid.NewGuid()}{extension}";
            var url = await _amazonS3Service.UploadFile(userDto.ConfigUrl.OpenReadStream(), keyName);
            user.ConfigUrl = url;
        }

        return await _userRepository.AddUserAsync(user);
    }
}
