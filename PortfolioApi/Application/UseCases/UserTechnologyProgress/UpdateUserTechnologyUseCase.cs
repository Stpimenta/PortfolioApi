using AutoMapper;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Infrastructure.Repository.Implementations;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.UserTechnologyProgress;

public class UpdateUserTechnologyUseCase
{
    private readonly IUserTechnologyRepository _repository;
    private readonly IMapper _mapper;
    private readonly IProjectRepository _projectRepository;
    public UpdateUserTechnologyUseCase(IUserTechnologyRepository repository, IMapper mapper,  IProjectRepository projectRepository)
    {
        _repository = repository;
        _mapper = mapper;
        _projectRepository = projectRepository;
    }

    public async Task ExecuteAsync(UpdateUserTechnologyProgressDto dto)
    {
        var techProgress = await _repository.GetAsync(dto.UserId, dto.TechId);
        if (techProgress is null)
            throw new NotFoundException(
                $"UserTechProgress not found for userId:{dto.UserId}, techId:{dto.TechId}");


        techProgress.Progress = dto.Progress;

        if (dto.ProjectIds == null || !dto.ProjectIds.Any())
        {

            techProgress.Projects.Clear();
        }
        else{
        
            var projects = await _projectRepository.GetProjectsByUserIdAndProjectIdsAsync(dto.UserId, dto.ProjectIds);

            var missingIds = dto.ProjectIds.Except(projects.Select(p => p.Id)).ToList();
            if (missingIds.Any())
                throw new NotFoundException($"Projects with Ids: {string.Join(", ", missingIds)} not found");

 
            var toRemove = techProgress.Projects.Where(p => !dto.ProjectIds.Contains(p.Id)).ToList();
            foreach (var project in toRemove)
            {
                techProgress.Projects.Remove(project);
            }

            var toAdd = projects.Where(p => techProgress.Projects.All(tp => tp.Id != p.Id)).ToList();
            foreach (var project in toAdd)
            {
                techProgress.Projects.Add(project);
            }
        }

        await _repository.UpdateAsync(techProgress);
    }
}