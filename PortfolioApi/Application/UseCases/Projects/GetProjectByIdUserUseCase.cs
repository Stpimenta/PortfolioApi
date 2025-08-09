using AutoMapper;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.Projects;

public class GetProjectByIdUserUseCase
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetProjectByIdUserUseCase(IProjectRepository projectRepository, IUserRepository userRepository, IMapper mapper)
    {
        _projectRepository = projectRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }
    public async Task<IEnumerable<GetProjectDto>> ExecuteAsync(int userId)
    {
        
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user is null)
            throw new NotFoundException($"user with id {userId} not found");
        
        var projects = await  _projectRepository.GetByUserIdAsync(userId);

        return _mapper.Map<IEnumerable<GetProjectDto>>(projects);
    }
    
}