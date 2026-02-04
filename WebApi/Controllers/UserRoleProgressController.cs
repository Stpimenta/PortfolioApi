using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Application.UseCases.UserRoleProgress;
using PortfolioApi.Domain.Entities;

namespace PortfolioApi.WebApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UserRoleProgressController : ControllerBase
{
    private readonly AddUserRoleProgressUseCase _addUseCase;
    private readonly UpdateUserRoleProgressUseCase _updateUseCase;
    private readonly DeleteUserRoleProgressUseCase _deleteUseCase;
    private readonly GetUserRoleProgressByUserUseCase _getByUserUseCase;

    public UserRoleProgressController(
        AddUserRoleProgressUseCase addUseCase,
        UpdateUserRoleProgressUseCase updateUseCase,
        DeleteUserRoleProgressUseCase deleteUseCase,
        GetUserRoleProgressByUserUseCase getByUserUseCase)
    {
        _addUseCase = addUseCase;
        _updateUseCase = updateUseCase;
        _deleteUseCase = deleteUseCase;
        _getByUserUseCase = getByUserUseCase;
    }

   
    [HttpGet("user/{userId}")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<GetUserRoleProgressDto>>> GetByUser(int userId)
    {
        var progressList = await _getByUserUseCase.ExecuteAsync(userId);
        return Ok(progressList); 
    }


    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] CreateUserRoleProgressDto dto)
    {
        await _addUseCase.ExecuteAsync(dto);
        return CreatedAtAction(nameof(GetByUser), new { userId = dto.UserId }, dto);
    }

 
    [HttpPut]
    [Authorize]
    public async Task<ActionResult> Update([FromBody] UpdateUserRoleProgressDto dto)
    {
        await _updateUseCase.ExecuteAsync(dto);
        return Ok(dto);
    }


    [HttpDelete("{userId}/{roleId}")]
    [Authorize]
    public async Task<ActionResult> Delete(int userId, int roleId)
    {
        await _deleteUseCase.ExecuteAsync(userId, roleId);
        return NoContent(); 
    }
}