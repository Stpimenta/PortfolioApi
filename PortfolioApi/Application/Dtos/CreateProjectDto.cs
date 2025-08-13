using System.ComponentModel.DataAnnotations;

namespace PortfolioApi.Application.Dtos;

public class CreateProjectDto
{
    [Required]
    [MaxLength(100)]
    public string? Name { get; set; } 
    [MaxLength(2000)]
    public string? Description { get; set; }
    public string? Download { get; set; }
    
    public string? Git { get; set; }
    public IFormFile? Config { get; set; }
    public IFormFile? Icon { get; set; }

    public List<IFormFile> Images { get; set; } = new();
    
    [Required]
    public int UserId { get; set; }
    public List<int> TechnologyIds { get; set; } = new();
}