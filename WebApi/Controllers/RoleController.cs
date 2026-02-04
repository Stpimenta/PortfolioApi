using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Application.UseCases.Roles;
using PortfolioApi.Domain.Entities;

namespace PortfolioApi.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly GetAllRolesUseCase _getAllRolesUseCase;
    private readonly GetRoleByIdUseCase _getRoleByIdUseCase;
    private readonly CreateRoleUseCase _createRoleUseCase;
    private readonly UpdateRoleUseCase _updateRoleUseCase;
    private readonly DeleteRoleUseCase _deleteRoleUseCase;

    public RolesController(
        GetAllRolesUseCase getAllRolesUseCase,
        GetRoleByIdUseCase getRoleByIdUseCase,
        CreateRoleUseCase createRoleUseCase,
        UpdateRoleUseCase updateRoleUseCase,
        DeleteRoleUseCase deleteRoleUseCase)
    {
        _getAllRolesUseCase = getAllRolesUseCase;
        _getRoleByIdUseCase = getRoleByIdUseCase;
        _createRoleUseCase = createRoleUseCase;
        _updateRoleUseCase = updateRoleUseCase;
        _deleteRoleUseCase = deleteRoleUseCase;
    }
    
   
    [HttpGet("GetAll")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<Role>>> GetAll()
    {
        var roles = await _getAllRolesUseCase.ExecuteAsync();
        return Ok(roles);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<RoleWithTechnologiesDto>> GetById(int id)
    {
        var role = await _getRoleByIdUseCase.ExecuteAsync(id);
        return Ok(role);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<int>> Create([FromBody] CreateRoleDto dto)
    {
        var id = await _createRoleUseCase.ExecuteAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateRoleDto dto)
    {
        await _updateRoleUseCase.ExecuteAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await _deleteRoleUseCase.ExecuteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}