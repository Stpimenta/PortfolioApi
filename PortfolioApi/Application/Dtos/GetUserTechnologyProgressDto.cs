namespace PortfolioApi.Application.Dtos;

public class GetUserTechnologyProgressDto
{
    public int UserId { get; set; }
    public int TechId { get; set; }
    public double Progress { get; set; }
    public GetTechnologyDto Tech { get; set; } = null!;
}