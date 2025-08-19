using System.Text.Json;
using AutoMapper;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Application.Services;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.Projects;

public class GetProjectByIdUserUseCase
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private const string BucketBaseUrl = "https://portfoliobucketts.s3.sa-east-1.amazonaws.com/";
    private readonly AmazonS3Service _amazonS3Service;
    public GetProjectByIdUserUseCase(IProjectRepository projectRepository, IUserRepository userRepository, IMapper mapper,  AmazonS3Service amazonS3Service)
    {
        _projectRepository = projectRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _amazonS3Service = amazonS3Service;
        
    }
    public async Task<IEnumerable<GetProjectDto>> ExecuteAsync(int userId)
    {
        
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user is null)
            throw new NotFoundException($"user with id {userId} not found");
        
        var projects = await  _projectRepository.GetByUserIdAsync(userId);
        var projectDtos = _mapper.Map<IEnumerable<GetProjectDto>>(projects);

        foreach (var project in projectDtos)
        {
            if (!string.IsNullOrEmpty(project.Config))
            {
                var jsonString = await _amazonS3Service.GetFileAsStringAsync(project.Config);
                project.ConfigJson = JsonDocument.Parse(jsonString);
            }
        }

        return projectDtos;
        
        
    }
    
}