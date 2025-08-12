using System.ComponentModel.DataAnnotations;

namespace PortfolioApi.Application.Dtos;

public class UpdateProjectDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    
    public string? Description { get; set; }
    public string? Download { get; set; }
    public string? Git { get; set; }
    public IFormFile? Icon { get; set; }
    public IFormFile? ConfigUrl { get; set; }

    [Required]
    public int UserId { get; set; }
    
    public List<int> TechnologyIds { get; set; } = new();
}