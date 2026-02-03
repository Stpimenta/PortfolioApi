using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PortfolioApi.Application.UseCases.Users;
using PortfolioApi.Application.Dtos; 

namespace PortfolioApi.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly LoginUserUseCase _loginUserUseCase;

        public AuthController(LoginUserUseCase loginUserUseCase)
        {
            _loginUserUseCase = loginUserUseCase;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            var tokenResponse = await _loginUserUseCase.ExecuteAsync(loginDto.Email, loginDto.Password);
            return Ok(tokenResponse);
        }
    }
}