using its.gamify.domains.Enums;

namespace its.gamify.domains.Entities;

public class CourseParticipation : BaseEntity
{
    public DateTime EnrolledDate { get; set; }
    public string Status { get; set; } = CourseStatusEnum.Enrolled.ToString();

    public Guid UserId { get; set; }
    public virtual User User { get; set; } = null!;
    public Guid CourseId { get; set; }
    public virtual Course Course { get; set; } = null!;
    public virtual CourseResult? CourseResult { get; set; }
    public ICollection<LearningProgress> LearningProgresses { get; set; } = new List<LearningProgress>();
    public virtual CourseReview? CourseReview { get; set; } 
}