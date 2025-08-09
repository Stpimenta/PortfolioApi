namespace PortfolioApi.Domain.Entities;

public class Project
{
    public int Id {get; set;}
    public string? Name {get; set;}
    public string? Description {get; set;}
    public string? Download {get; set;}
    public string? Git { get; set;}
    public string? Icon {get; set;}
    
    public int UserId { get; set; } 
    public User User { get; set; }
    public ICollection<Technology> Technologies   { get; set; } = new List<Technology>();
}


