using PortfolioApi.Domain.Entities;

namespace PortfolioApi.Application.Dtos;

public class GetUserRoleProgressDto
{
    public int UserId { get; set; }
    public int RoleId { get; set; }
    public double Progress { get; set; }
    public RoleDto Role { get; set; }
    public List<GetProjectDto> Projects { get; set; } = new();
}