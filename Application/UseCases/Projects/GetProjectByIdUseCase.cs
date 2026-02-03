using System.Text.Json;
using AutoMapper;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Application.Services;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.Projects;

public class GetProjectByIdUseCase
{
    private readonly IProjectRepository _repository;
    private readonly IMapper _mapper;
    private readonly AmazonS3Service _amazonS3Service;
    public GetProjectByIdUseCase(IProjectRepository repository, IMapper mapper, AmazonS3Service amazonS3Service)
    {
        _repository = repository;
        _mapper = mapper;
        _amazonS3Service = amazonS3Service;
    }

    public async Task<GetProjectDto> ExecuteAsync(int id)
    {
        var project = await _repository.GetByIdAsync(id);
        if (project == null)
            throw new NotFoundException($"Project with id {id} not found.");
        
        var projectDto = _mapper.Map<GetProjectDto>(project);
        if (!string.IsNullOrEmpty(project.Config))
        {
            var jsonString = await _amazonS3Service.GetFileAsStringAsync(project.Config);
            projectDto.ConfigJson = JsonDocument.Parse(jsonString);
        }

        return projectDto;
    }
}