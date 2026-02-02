using System.Text.Json;

namespace PortfolioApi.Application.Dtos;
using System.Collections.Generic;

public class GetProjectDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public GetUserDto? User { get; set; }
    public List<string>? Images { get; set; }
    public List<TechnologyDto> Technologies { get; set; } = new();
    public string? Config { get; set; } 
    public JsonDocument? ConfigJson { get; set; }
}


