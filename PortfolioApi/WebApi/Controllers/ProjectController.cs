using Microsoft.AspNetCore.Mvc;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Application.UseCases.Projects;
using PortfolioApi.Domain.Entities;

namespace PortfolioApi.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly GetAllProjectsUseCase _getAllProjectsUseCase;
    private readonly GetProjectByIdUseCase _getProjectByIdUseCase;
    private readonly AddProjectUseCase _createProjectUseCase;
    private readonly UpdateProjectUseCase _updateProjectUseCase;
    private readonly DeleteProjectUseCase _deleteProjectUseCase;

    public ProjectController(
        GetAllProjectsUseCase getAllProjectsUseCase,
        GetProjectByIdUseCase getProjectByIdUseCase,
        AddProjectUseCase createProjectUseCase,
        UpdateProjectUseCase updateProjectUseCase,
        DeleteProjectUseCase deleteProjectUseCase)
    {
        _getAllProjectsUseCase = getAllProjectsUseCase;
        _getProjectByIdUseCase = getProjectByIdUseCase;
        _createProjectUseCase = createProjectUseCase;
        _updateProjectUseCase = updateProjectUseCase;
        _deleteProjectUseCase = deleteProjectUseCase;
    }


    [HttpGet("GetAll")]
    public async Task<ActionResult<IEnumerable<Project>>> GetAll()
    {
        var projects = await _getAllProjectsUseCase.ExecuteAsync();
        return Ok(projects);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Project>> GetById(int id)
    {
        var project = await _getProjectByIdUseCase.ExecuteAsync(id);
        if (project == null) return NotFound();
        return Ok(project);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateProjectDto dto)
    {
        var id = await _createProjectUseCase.ExecuteAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateProjectDto dto)
    {
        await _updateProjectUseCase.ExecuteAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await _deleteProjectUseCase.ExecuteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}
