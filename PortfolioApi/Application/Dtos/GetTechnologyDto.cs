namespace PortfolioApi.Application.Dtos;

public class GetTechnologyDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? IconPath { get; set; }
}