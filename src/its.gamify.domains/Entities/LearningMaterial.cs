using its.gamify.domains.Enums;

namespace its.gamify.domains.Entities;

public class LearningMaterial : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = LearningMaterialType.Undefined.ToString();// e.g., Video, Article, Quiz
    public string Url { get; set; } = string.Empty; // Link to the material
    
    public Guid CourseId { get; set; }
    public virtual Course Course { get; set; } = null!; // Navigation property to the course this material belongs to

}   