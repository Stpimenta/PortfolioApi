namespace PortfolioApi.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }

    public ICollection<Project> Projects { get; set; } = new List<Project>();
    public ICollection<UserTechProgress> UserTechnologies { get; set; } = new List<UserTechProgress>();
    public ICollection<UserRoleProgress> UserRoles { get; set; } = new List<UserRoleProgress>();
}
