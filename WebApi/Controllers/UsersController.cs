using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Application.UseCases.Users;
using PortfolioApi.Domain.Entities;

namespace PortfolioApi.WebApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase 
{  
    private readonly GetAllUsersUseCase _getAllUsersUseCase;
    private readonly AddUserUseCase _addUserUseCase;
    private readonly GetUserByIdUseCase _getUserByIdUseCase;
    private readonly UpdateUserUseCase _updateUserByIdUseCase;
    private readonly DeleteUserUseCase _deleteUserByIdUseCase;

    public UsersController(GetAllUsersUseCase getAllUsersUseCase, AddUserUseCase addUserUseCase, GetUserByIdUseCase 
                                getUserByIdUseCase, UpdateUserUseCase updateUserByIdUseCase,  DeleteUserUseCase deleteUserByIdUseCase)
    {
        _getAllUsersUseCase = getAllUsersUseCase;
        _addUserUseCase = addUserUseCase;
        _getUserByIdUseCase = getUserByIdUseCase;
        _updateUserByIdUseCase = updateUserByIdUseCase;
        _deleteUserByIdUseCase = deleteUserByIdUseCase;
    }
    
    [HttpGet("GetAll")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<GetUserDto>>> GetAll()
    {
        var users = await _getAllUsersUseCase.ExecuteAsync();
        return Ok(users);
    }
    
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<GetUserDto>> GetById(int id)
    {
        var user = await _getUserByIdUseCase.ExecuteAsync(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<int>> Post([FromForm] CreateUserDto user)
    {
        var userId = await _addUserUseCase.ExecuteAsync(user);
        return CreatedAtAction(nameof(GetById), new { id = userId }, userId);
    }
    
    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<int>> UpdateUser([FromForm] UpdateUserDto userDto, int id)
    {
        var user = await _updateUserByIdUseCase.ExecuteAsync(id,userDto);
        if (user == null) return NotFound();
        return Ok(user);
    }
    
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult<bool>> DeleteUser(int id)
    {
        var user = await _deleteUserByIdUseCase.ExecuteAsync(id);
        return Ok(user);
    }
   
    
}