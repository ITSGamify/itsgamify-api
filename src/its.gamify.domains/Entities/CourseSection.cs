namespace its.gamify.domains.Entities;

public class CourseSection : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public int OrderedNumber { get; set; }

    public Guid CourseId { get; set; }
    public virtual Course Course { get; set; } = null!; // Navigation property to the course this section belongs to
    public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>(); // Collection of lessons in this section
    
    
}