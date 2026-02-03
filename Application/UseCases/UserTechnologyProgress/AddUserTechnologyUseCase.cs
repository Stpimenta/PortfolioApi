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
    private readonly IProjectRepository _projectRepository;

    public AddUserTechnologyUseCase(
        IUserTechnologyRepository repository, 
        IMapper mapper, 
        IUserRepository userRepository,
        ITechnologyRepository techRepository,
        IProjectRepository projectRepository)
    {
        _repository = repository;
        _mapper = mapper;
        _userRepository = userRepository;
        _techRepository = techRepository;
        _projectRepository = projectRepository;
    }

    public async Task ExecuteAsync(CreateUserTechnologyProgressDto dto)
    {
        var exist = await _repository.ExistsAsync(dto.UserId, dto.TechId);
        if (exist)
            throw new BusinessException("User technology progress already exist");

        var user = await _userRepository.GetUserByIdAsync(dto.UserId);
        if (user == null)
            throw new NotFoundException("User not found");

        var tech = await _techRepository.GetByIdAsync(dto.TechId);
        if (tech == null)
            throw new NotFoundException("Technology not found");

        var techProgress = _mapper.Map<Domain.Entities.UserTechProgress>(dto);
        
        if (dto.ProjectIds != null && dto.ProjectIds.Any())
        {
            var projects = await _projectRepository
                .GetProjectsByUserIdAndProjectIdsAsync(dto.UserId, dto.ProjectIds);

            var missingIds = dto.ProjectIds.Except(projects.Select(p => p.Id)).ToList();
            if (missingIds.Any())
                throw new NotFoundException($"Projects with Ids: {string.Join(", ", missingIds)} not found");

            foreach (var project in projects)
            {
                techProgress.Projects.Add(project);
            }
        }

        await _repository.AddAsync(techProgress);
    }
}