namespace its.gamify.domains.Entities;

public class Quiz : BaseEntity
{
    public double TotalMarks { get; set; } = 0.0;
    public double PassedMarks { get; set; } = 0.0;
    public int TotalQuestions { get; set; } = 0;
    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
    public virtual ICollection<QuizResult> QuizResults { get; set; } = new List<QuizResult>();
    public Guid CourseSectionId { get; set; }
    public virtual CourseSection CourseSection { get; set; } = null!;
    public Guid ChallengIdId { get; set; }
    public virtual Challenge Challenge { get; set; } = null!;
}