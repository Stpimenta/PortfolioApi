namespace PortfolioApi.Domain.Entities;

public class UserRoleProgress
{
    public int UserId { get; set; }
    public User? User { get; set; } 
    public int RoleId { get; set; }
    public Role? Role { get; set; }
    public double Progress { get; set; }
    
    public ICollection<Project> Projects { get; set; } =  new List<Project>();
    
}