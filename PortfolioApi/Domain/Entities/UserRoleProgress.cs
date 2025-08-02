namespace PortfolioApi.Domain.Entities;

public class UserRoleProgress
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;

    public double Progress { get; set; }
}