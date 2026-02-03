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
    private readonly GetProjectByIdUserUseCase _getProjectByIdUserUseCase;
    private readonly AddImageUseCase _addImageUseCase;
    private readonly DeleteImageUseCase _deleteImageUseCase;

    public ProjectController(
        GetAllProjectsUseCase getAllProjectsUseCase,
        GetProjectByIdUseCase getProjectByIdUseCase,
        AddProjectUseCase createProjectUseCase,
        UpdateProjectUseCase updateProjectUseCase,
        DeleteProjectUseCase deleteProjectUseCase,
        GetProjectByIdUserUseCase getProjectByIdUserUseCase,
        AddImageUseCase addImageUseCase,
        DeleteImageUseCase deleteImageUseCase)
    {
        _getAllProjectsUseCase = getAllProjectsUseCase;
        _getProjectByIdUseCase = getProjectByIdUseCase;
        _createProjectUseCase = createProjectUseCase;
        _updateProjectUseCase = updateProjectUseCase;
        _deleteProjectUseCase = deleteProjectUseCase;
        _getProjectByIdUserUseCase = getProjectByIdUserUseCase;
        _addImageUseCase = addImageUseCase;
        _deleteImageUseCase = deleteImageUseCase;
    }


    [HttpGet("GetAll")]
    public async Task<ActionResult<IEnumerable<GetProjectDto>>> GetAll()
    {
        var projects = await _getAllProjectsUseCase.ExecuteAsync();
        return Ok(projects);
    }
    
    [HttpGet("GetByUserId/{userId}")]
    public async Task<ActionResult<IEnumerable<GetProjectDto>>> GetByUserId(int  userId)
    {
        var projects = await _getProjectByIdUserUseCase.ExecuteAsync(userId);
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
    public async Task<ActionResult<int>> Create( [FromForm] CreateProjectDto dto)
    {
        var id = await _createProjectUseCase.ExecuteAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromForm] UpdateProjectDto dto)
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
    
    [HttpPost ("/images/{projectId}")]
    public async Task<ActionResult<int>> AddImages ( int projectId, [FromForm] List<IFormFile> images)
    {
        await  _addImageUseCase.ExecuteAsync(projectId, images);
        return NoContent();
    }
    
    [HttpDelete("/images/{projectId}/{imageUrl}")]
    public async Task<ActionResult> RemoveImage(int projectId, string imageUrl)
    {
       await  _deleteImageUseCase.ExecuteAsync(projectId, imageUrl);
       return NoContent();
    }
}
