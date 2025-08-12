namespace PortfolioApi.Application.Dtos;

public class GetProjectDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Download { get; set; }
    public string? Git { get; set; }
    public string? Icon { get; set; }
    public GetUserDto? User { get; set; }
    public string? ConfigUrl { get; set; }
    public List<string>? Images { get; set; }
    public List<TechnologyDto> Technologies { get; set; } = new();
}