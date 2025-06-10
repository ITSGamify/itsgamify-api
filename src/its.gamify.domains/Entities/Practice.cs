namespace its.gamify.domains.Entities;

public class Practice : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid CourseId { get; set; }
    public virtual Course Course { get; set; } = null!;
    public Guid CourseSectionId { get; set; }
    public virtual CourseSection CourseSection { get; set; } = null!;
    public virtual ICollection<PracticeTag> PracticeTags { get; set; } = [];
    
}