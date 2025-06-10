namespace its.gamify.domains.Entities;

public class PracticeTag : BaseEntity
{
    public string TagName { get; set; } = string.Empty;
    public Guid PracticeId { get; set; }
    public virtual Practice Practice { get; set; } = null!;
    
}