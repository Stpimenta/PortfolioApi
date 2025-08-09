using Microsoft.AspNetCore.Mvc;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Application.UseCases.Icons;
using PortfolioApi.Domain.Entities;

namespace PortfolioApi.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IconsController : ControllerBase
{
    private readonly GetAllIconsUseCase _getAllIconsUseCase;
    private readonly GetIconByIdUseCase _getIconByIdUseCase;
    private readonly AddIconUseCase _createIconUseCase;
    private readonly UpdateIconUseCase _updateIconUseCase;
    private readonly DeleteIconUseCase _deleteIconUseCase;

    public IconsController(
        GetAllIconsUseCase getAllIconsUseCase,
        GetIconByIdUseCase getIconByIdUseCase,
        AddIconUseCase createIconUseCase,
        UpdateIconUseCase updateIconUseCase,
        DeleteIconUseCase deleteIconUseCase)
    {
        _getAllIconsUseCase = getAllIconsUseCase;
        _getIconByIdUseCase = getIconByIdUseCase;
        _createIconUseCase = createIconUseCase;
        _updateIconUseCase = updateIconUseCase;
        _deleteIconUseCase = deleteIconUseCase;
    }

    [HttpGet("GetAll")]
    public async Task<ActionResult<IEnumerable<Icons>>> GetAll()
    {
        var icons = await _getAllIconsUseCase.ExecuteAsync();
        return Ok(icons);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Icons>> GetById(int id)
    {
        var icon = await _getIconByIdUseCase.ExecuteAsync(id);
        return Ok(icon);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Post([FromForm] CreateIconDto dto)
    {
        var iconId = await _createIconUseCase.ExecuteAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = iconId }, iconId);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<int>> Put(int id, [FromForm] UpdateIconDto dto)
    {
        var iconId = await _updateIconUseCase.ExecuteAsync(id, dto);
        return Ok(iconId);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        var deleted = await _deleteIconUseCase.ExecuteAsync(id);
        return Ok(deleted);
    }
}