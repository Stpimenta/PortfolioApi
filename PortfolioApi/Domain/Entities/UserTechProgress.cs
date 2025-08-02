namespace PortfolioApi.Domain.Entities;

public class UserTechProgress
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int TechId { get; set; }
    public Technology Tech { get; set; } = null!;
    public double Progress { get; set; }
}