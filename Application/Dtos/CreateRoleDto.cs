using System.ComponentModel.DataAnnotations;

namespace PortfolioApi.Application.Dtos;

public class CreateRoleDto
{
    [Required]
    public string Name { get; set; } = null!;
    
    public List<int> TechnologyIds { get; set; } = new();

    public int? IconId { get; set; }
}