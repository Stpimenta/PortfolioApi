using AutoMapper;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Infrastructure.Repository.Implementations;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.UserTechnologyProgress;

public class AddUserTechnologyUseCase
{
    private readonly IUserTechnologyRepository _repository;
    private readonly IUserRepository _userRepository;
    private readonly ITechnologyRepository _techRepository;
    private readonly IMapper _mapper;

    public AddUserTechnologyUseCase(
        IUserTechnologyRepository repository, 
        IMapper mapper, 
        IUserRepository userRepository,
        ITechnologyRepository techRepository)
    {
        _repository = repository;
        _mapper = mapper;
        _userRepository = userRepository;
        _techRepository = techRepository;
    }

    public async Task ExecuteAsync(CreateUserTechnologyProgressDto dto)
    {
        var exist = await _repository.ExistsAsync(dto.UserId, dto.TechId);
        if (exist)
        {
            throw new BusinessException("User technology progress already exist");
        }

        var user = await _userRepository.GetUserByIdAsync(dto.UserId);
        if (user == null)
            throw new NotFoundException("User not found");

        var tech = await _techRepository.GetByIdAsync(dto.TechId);
        if (tech == null)
            throw new NotFoundException("Technology not found");

        var techProgress = _mapper.Map<Domain.Entities.UserTechProgress>(dto);
        await _repository.AddAsync(techProgress);
    }
}