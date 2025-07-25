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

    public UsersController(GetAllUsersUseCase getAllUsersUseCase, AddUserUseCase addUserUseCase, GetUserByIdUseCase getUserByIdUseCase)
    {
        _getAllUsersUseCase = getAllUsersUseCase;
        _addUserUseCase = addUserUseCase;
        _getUserByIdUseCase = getUserByIdUseCase;
    }
    
    [HttpGet("GetAll")]
    public async Task<ActionResult<IEnumerable<User>>> GetAll()
    {
        var users = await _getAllUsersUseCase.ExecuteAsync();
        return Ok(users);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Post([FromBody] CreateUserDto user)
    {
        var userId = await _addUserUseCase.ExecuteAsync(user);
        return CreatedAtAction(nameof(GetById), new { id = userId }, userId);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetById(int id)
    {
        var user = await _getUserByIdUseCase.ExecuteAsync(id);
        if (user == null) return NotFound();
        return Ok(user);
    }
    
}