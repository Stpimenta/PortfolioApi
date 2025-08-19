using System.Text.Json;

namespace PortfolioApi.Application.Dtos;

public class GetUserDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public String? Config { get; set; }
    public JsonDocument? ConfigJson { get; set; }
    public List<string> ProjectNames { get; set; } = new();
}