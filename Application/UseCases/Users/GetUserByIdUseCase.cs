using System.Text.Json;
using AutoMapper;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Application.Services;
using PortfolioApi.Domain.Entities;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.Users;

public class GetUserByIdUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly AmazonS3Service _amazonS3Service;
    public GetUserByIdUseCase(IUserRepository userRepository,  IMapper mapper,  AmazonS3Service amazonS3Service)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _amazonS3Service = amazonS3Service;
    }

    public async Task<GetUserDto?> ExecuteAsync(int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user is null)
        {
            throw new NotFoundException($"user {userId} not found");
        }

        var userDto = _mapper.Map<GetUserDto>(user);
        if (!string.IsNullOrEmpty(userDto.Config))
        {
            var json = await _amazonS3Service.GetFileAsStringAsync(user.Config);
            userDto.ConfigJson = JsonDocument.Parse(json);
        }
        
        return userDto;
    }
}