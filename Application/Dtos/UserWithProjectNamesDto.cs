namespace PortfolioApi.Application.Dtos;

public class UserWithProjectNamesDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    
    public List<string> ProjectNames { get; set; } = new();
}