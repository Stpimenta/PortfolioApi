using System.ComponentModel.DataAnnotations;

namespace PortfolioApi.Application.Dtos;

public class UpdateUserRoleProgressDto
{
    [Required]
    public int UserId { get; set; }
    
    [Required]
    public int RoleId { get; set; }
    
    [Range(0, 100)]
    public double Progress { get; set; }
    
    public List<int> ProjectIds { get; set; } = new ();
}