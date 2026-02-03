namespace PortfolioApi.Application.Dtos;

public class TechnologySummaryDto
{
    public string Name { get; set; } = null!;
    public string? IconPath { get; set; } 
}

public class RoleWithTechnologiesDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? IconPath { get; set; } 
    public List<TechnologySummaryDto> Technologies { get; set; } = new();
}