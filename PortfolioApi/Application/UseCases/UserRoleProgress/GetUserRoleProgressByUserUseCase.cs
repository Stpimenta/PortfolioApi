using AutoMapper;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.UserRoleProgress;

public class GetUserRoleProgressByUserUseCase
{
    private readonly IUserRoleProgressRepository _repository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    
    public GetUserRoleProgressByUserUseCase(IUserRoleProgressRepository repository, IUserRepository userRepository,  IMapper mapper)
    {
        _repository = repository;
        _userRepository = userRepository;
        _mapper = mapper;

    }

    public async Task<IEnumerable<GetUserRoleProgressDto>> ExecuteAsync(int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
            throw new NotFoundException("User not found");
        
        var progresses = await _repository.getByUserAsync(userId);
        if (!progresses.Any())
            throw new NotFoundException("No progress found for user.");

        return _mapper.Map<IEnumerable<GetUserRoleProgressDto>>(progresses);
    }
}