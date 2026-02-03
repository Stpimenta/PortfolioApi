using System.Text.Json;
using AutoMapper;
using AutoMapper.QueryableExtensions.Impl;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Application.Services;
using PortfolioApi.Infrastructure.Repository.Interfaces;

namespace PortfolioApi.Application.UseCases.Projects;

public class GetAllProjectsUseCase
{
    private readonly IProjectRepository _repository;
    private  readonly IMapper _mapper;
    private const string BucketBaseUrl = "https://portfoliobucketts.s3.sa-east-1.amazonaws.com/";
    private readonly HttpClient _httpClient;
    private readonly AmazonS3Service _amazonS3Service;
    public GetAllProjectsUseCase(IProjectRepository repository, IMapper mapper,  HttpClient httpClient, AmazonS3Service amazonS3Service)
    {
        _repository = repository;
        _mapper = mapper;
        _httpClient = httpClient;
        _amazonS3Service = amazonS3Service;
        
    }

    public async Task<IEnumerable<GetProjectDto>> ExecuteAsync()
    {
        var  projects = await _repository.GetAllAsync();
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