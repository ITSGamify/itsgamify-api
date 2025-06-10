namespace its.gamify.domains.Entities;

public class QuizResult : BaseEntity
{
    public double Score { get; set; } = 0.0;
    public DateTime CompletedDate { get; set; } = DateTime.Now;
    public bool IsPassed { get; set; } = false;
    public ICollection<QuizAnswer> QuizAnswers { get; set; } = new List<QuizAnswer>();
    public LearningProgress? LearningProgress { get; set; } = null!;
}