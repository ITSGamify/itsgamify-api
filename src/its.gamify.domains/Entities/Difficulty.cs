namespace its.gamify.domains.Entities;

public class Difficulty : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public virtual Course? Course { get; set; } 
    
}