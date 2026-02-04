using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Application.UseCases.Technologies;
using PortfolioApi.Domain.Entities;

namespace PortfolioApi.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TechnologyController : ControllerBase
    {
        private readonly GetAllTechnologiesUseCase _getAllTechnologiesUseCase;
        private readonly AddTechnologyUseCase _addTechnologyUseCase;
        private readonly GetTechnologyByIdUseCase _getTechnologyByIdUseCase;
        private readonly UpdateTechnologyUseCase _updateTechnologyUseCase;
        private readonly DeleteTechnologyUseCase _deleteTechnologyUseCase;

        public TechnologyController(
            GetAllTechnologiesUseCase getAllTechnologiesUseCase,
            AddTechnologyUseCase addTechnologyUseCase,
            GetTechnologyByIdUseCase getTechnologyByIdUseCase,
            UpdateTechnologyUseCase updateTechnologyUseCase,
            DeleteTechnologyUseCase deleteTechnologyUseCase)
        {
            _getAllTechnologiesUseCase = getAllTechnologiesUseCase;
            _addTechnologyUseCase = addTechnologyUseCase;
            _getTechnologyByIdUseCase = getTechnologyByIdUseCase;
            _updateTechnologyUseCase = updateTechnologyUseCase;
            _deleteTechnologyUseCase = deleteTechnologyUseCase;
        }

        [HttpGet("GetAll")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Technology>>> GetAll()
        {
            var techs = await _getAllTechnologiesUseCase.ExecuteAsync();
            return Ok(techs);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Technology>> GetById(int id)
        {
            var tech = await _getTechnologyByIdUseCase.ExecuteAsync(id);
            return Ok(tech);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<int>> Post([FromBody] CreateTechnologyDto dto)
        {
            var techId = await _addTechnologyUseCase.ExecuteAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = techId }, techId);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<int>> Update([FromBody] UpdateTechnologyDto dto, int id)
        {
            var updatedId = await _updateTechnologyUseCase.ExecuteAsync(id, dto);
            return Ok(updatedId);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var deleted = await _deleteTechnologyUseCase.ExecuteAsync(id);
            return Ok(deleted);
        }
    }
}
