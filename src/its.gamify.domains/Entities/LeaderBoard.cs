namespace its.gamify.domains.Entities;

public class LeadearBoard : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ICollection<User>? Users { get; set; }
    
}