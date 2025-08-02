namespace PortfolioApi.Domain.Entities;

public class Role
{
    public int Id { get; set; }
    public string? Name { get; set; }
    
    public int? IconId { get; set; } 
    public Icons? Icon { get; set; }  
    public ICollection<Technology> Technologies   { get; set; } = new List<Technology>();
    public ICollection<UserRoleProgress> UserRoles { get; set; } = new List<UserRoleProgress>();
    
}