namespace its.gamify.domains.Entities;

public class CourseReview : BaseEntity
{
    public double Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime ReviewdDate { get; set; } = DateTime.Now;
    public Guid CourseId { get; set; }
    public virtual Course Course { get; set; } = null!;
    public Guid CourseParticipationId { get; set; }
    public virtual CourseParticipation CourseParticipation { get; set; } = null!;
}