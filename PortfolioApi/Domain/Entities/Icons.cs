namespace PortfolioApi.Domain.Entities;

public class Icons
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Path { get; set; }
    
    public ICollection<Technology> Technologies   { get; set; } = new List<Technology>();
    public ICollection<Role> Roles   { get; set; } = new List<Role>();
}