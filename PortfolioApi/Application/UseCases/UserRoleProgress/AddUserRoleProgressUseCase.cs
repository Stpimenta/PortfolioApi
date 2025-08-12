using AutoMapper;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.UserRoleProgress;

public class AddUserRoleProgressUseCase
{
    private readonly IUserRoleProgressRepository _repository;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;
    private readonly IProjectRepository _projectRepository;
    
    public  AddUserRoleProgressUseCase(IUserRoleProgressRepository repository,  IMapper mapper,  
        IUserRepository userRepository, IRoleRepository roleRepository,  IProjectRepository projectRepository)
    {
        _repository = repository;
        _mapper = mapper;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _projectRepository = projectRepository;
        
    }

    public async Task ExecuteAsync(CreateUserRoleProgressDto dto)
    {
        var exist = await _repository.ExistsAsync(dto.UserId, dto.RoleId);
        if (exist)
        {
            throw new BusinessException("User role progress already exist");
        }

        var user = await _userRepository.GetUserByIdAsync(dto.UserId);
        if (user == null)
            throw new NotFoundException("User not found");

        var role = await _roleRepository.GetByIdAsync(dto.RoleId);
        if (role == null)
            throw new NotFoundException("Role not found");
    
        var roleProgress = _mapper.Map<Domain.Entities.UserRoleProgress>(dto);
        
        
        if (dto.ProjectIds.Any())
        { 
            var projects = await _projectRepository.GetProjectsByUserIdAndProjectIdsAsync
                (dto.UserId, dto.ProjectIds);


            var missingIds =
                dto.ProjectIds.Except(projects.Select(p => p.Id))
                    .ToList(); 
            
            if(missingIds.Any())
                throw new NotFoundException($"Projects with Ids: {string.Join(", ", missingIds)} not found");

            foreach (var project in projects)
            {
                roleProgress.Projects.Add(project);
            }
            
        }
        
        await _repository.AddAsync(roleProgress);
    }
}