using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Application.UseCases.UserTechnologyProgress;

namespace PortfolioApi.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserTechnologyProgressController : ControllerBase
{
    private readonly AddUserTechnologyUseCase _addUseCase;
    private readonly UpdateUserTechnologyUseCase _updateUseCase;
    private readonly DeleteUserTechnologyUseCase _deleteUseCase;
    private readonly GetUserTechnologyUseCase _getByUserUseCase;

    public UserTechnologyProgressController(
        AddUserTechnologyUseCase addUseCase,
        UpdateUserTechnologyUseCase updateUseCase,
        DeleteUserTechnologyUseCase deleteUseCase,
        GetUserTechnologyUseCase getByUserUseCase)
    {
        _addUseCase = addUseCase;
        _updateUseCase = updateUseCase;
        _deleteUseCase = deleteUseCase;
        _getByUserUseCase = getByUserUseCase;
    }

    [HttpGet("user/{userId}")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<GetUserTechnologyProgressDto>>> GetByUser(int userId)
    {
        var progressList = await _getByUserUseCase.ExecuteAsync(userId);
        return Ok(progressList);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] CreateUserTechnologyProgressDto dto)
    {
        await _addUseCase.ExecuteAsync(dto);
        return CreatedAtAction(nameof(GetByUser), new { userId = dto.UserId }, dto);
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult> Update([FromBody] UpdateUserTechnologyProgressDto dto)
    {
        await _updateUseCase.ExecuteAsync(dto);
        return Ok(dto);
    }

    [HttpDelete("{userId}/{techId}")]
    [Authorize]
    public async Task<ActionResult> Delete(int userId, int techId)
    {
        await _deleteUseCase.ExecuteAsync(userId, techId);
        return NoContent();
    }
}
