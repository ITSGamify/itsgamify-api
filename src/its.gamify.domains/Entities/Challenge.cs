namespace its.gamify.domains.Entities;

public class Challenge : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    #region Navigation Properties
    public virtual ICollection<ChallengeParticipation> ChallengeParticipations { get; set; } = [];
    public virtual ICollection<EmployeeChallenge> EmployeeChallenges { get; set; } = [];
    public virtual ICollection<Quiz> Quizzes { get; set; } = [];
    #endregion
}