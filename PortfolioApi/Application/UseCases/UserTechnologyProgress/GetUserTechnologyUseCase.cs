using AutoMapper;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.UserTechnologyProgress;

public class GetUserTechnologyUseCase
{
    private readonly IUserTechnologyRepository _repository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUserTechnologyUseCase(
        IUserTechnologyRepository repository, 
        IUserRepository userRepository, 
        IMapper mapper)
    {
        _repository = repository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetUserTechnologyProgressDto>> ExecuteAsync(int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
            throw new NotFoundException("User not found");
        
        var progresses = await _repository.GetByUserAsync(userId);
        if (!progresses.Any())
            throw new NotFoundException("No technology progress found for user.");

        return _mapper.Map<IEnumerable<GetUserTechnologyProgressDto>>(progresses);
    }
}