namespace PortfolioApi.Domain.Entities;

public class Technology
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public int? IconId { get; set; }
    public Icons? Icon { get; set; } 

    public ICollection<Role> Roles { get; set; } = new List<Role>();
    public ICollection<Project> Projects { get; set; } = new List<Project>();
    public ICollection<UserTechProgress> UserTechnologies { get; set; } = new List<UserTechProgress>();
}
