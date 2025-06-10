namespace its.gamify.domains.Entities;

public class Question : BaseEntity
{
    public string Content { get; set; } = string.Empty;
    public string OptionA { get; set; } = string.Empty;
    public string OptionB { get; set; } = string.Empty;
    public string OptionC { get; set; } = string.Empty;
    public string OptionD { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; } = string.Empty;
    public string Explanation { get; set; } = string.Empty;
    public Guid QuestionBankId { get; set; }
    public virtual QuestionBank QuestionBank { get; set; } = null!;
    public Guid QuizId { get; set; }
    public virtual Quiz Quiz { get; set; } = null!;
    public ICollection<QuizAnswer> QuizAnswers { get; set; } = new List<QuizAnswer>();
    

}