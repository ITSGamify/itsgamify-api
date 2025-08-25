namespace its.gamify.domains.Entities;

public class CourseResult : BaseEntity
{
    public double Scrore { get; set; } = 0.0;
    public bool IsPassed { get; set; } = false;
    public DateTime CompletedDate { get; set; } = DateTime.UtcNow;
    public string CourseNumber { get; set; } = string.Empty;

    #region Relationship Configuration 
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = null!;

    public Guid CourseId { get; set; }
    public virtual Course Course { get; set; } = null!;
    public Guid CourseParticipationId { get; set; }
    public virtual CourseParticipation CourseParticipation { get; set; } = null!;
    #endregion
}