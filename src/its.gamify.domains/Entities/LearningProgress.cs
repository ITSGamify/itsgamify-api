namespace its.gamify.domains.Entities;

public class LearningProgress : BaseEntity
{
    public double Percentage { get; set; } = 0.0;
    public DateTime LastAccessed { get; set; } = DateTime.Now;
    public Guid CourseParticipationId { get; set; }
    public virtual CourseParticipation CourseParticipation { get; set; } = null!;

    public virtual QuizResult? QuizResult { get; set; } = null!;
    public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}