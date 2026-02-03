using AutoMapper;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Infrastructure.Repository.Implementations;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.UserRoleProgress;

public class UpdateUserRoleProgressUseCase
{
    private readonly IUserRoleProgressRepository _repository;
    private readonly IMapper _mapper;
    private readonly IProjectRepository _projectRepository;
    
    public UpdateUserRoleProgressUseCase(
        IUserRoleProgressRepository repository,  
        IMapper mapper,
        IProjectRepository projectRepository)
    {
        _repository = repository;
        _mapper = mapper;
        _projectRepository = projectRepository;
    }
    
    public async Task ExecuteAsync(UpdateUserRoleProgressDto dto)
    {
        var roleProgress = await _repository.GetAsync(dto.UserId, dto.RoleId);
        if (roleProgress is null)
            throw new NotFoundException($"UserRoleProgress not found for userId:{dto.UserId}, roleId:{dto.RoleId}");

        roleProgress.Progress = dto.Progress;

        if (dto.ProjectIds == null || !dto.ProjectIds.Any())
        {
            roleProgress.Projects.Clear();
        }
        else
        {
            var projects = await _projectRepository.GetProjectsByUserIdAndProjectIdsAsync(dto.UserId, dto.ProjectIds);

            var missingIds = dto.ProjectIds.Except(projects.Select(p => p.Id)).ToList();
            if (missingIds.Any())
                throw new NotFoundException($"Projects with Ids: {string.Join(", ", missingIds)} not found");

            var toRemove = roleProgress.Projects.Where(p => !dto.ProjectIds.Contains(p.Id)).ToList();
            foreach (var project in toRemove)
            {
                roleProgress.Projects.Remove(project);
            }

            var toAdd = projects.Where(p => roleProgress.Projects.All(rp => rp.Id != p.Id)).ToList();
            foreach (var project in toAdd)
            {
                roleProgress.Projects.Add(project);
            }
        }

        await _repository.UpdateAsync(roleProgress);
    }


}