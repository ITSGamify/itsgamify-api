namespace its.gamify.domains.Entities;

public class QuestionBank : BaseEntity
{
    public virtual ICollection<Question>? Questions { get; set; }
    public Guid CourseId { get; set; }
    public virtual Course Course { get; set; } = null!;
    
}