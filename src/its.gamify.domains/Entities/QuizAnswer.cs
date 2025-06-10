namespace its.gamify.domains.Entities;

public class QuizAnswer : BaseEntity
{
    public string Answer { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public Guid QuestionId { get; set; }
    public virtual Question Question { get; set; } = null!;

    public Guid QuizResultId { get; set; }
    public virtual QuizResult QuizResult { get; set; } = null!;
}