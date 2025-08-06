using System.ComponentModel.DataAnnotations;

namespace PortfolioApi.Application.Dtos;

public class UpdateUserTechnologyProgressDto
{
    [Required]
    public int UserId { get; set; }
    [Required]
    public int TechId { get; set; }
    [Range(0,100)]
    public double Progress { get; set; }
}
