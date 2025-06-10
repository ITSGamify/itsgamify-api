namespace its.gamify.domains.Entities;

public class EmployeeChallenge : BaseEntity
{
    public string Status { get; set; } = string.Empty;
    public DateTime? AttempatedDate { get; set; }

    public Guid UserId { get; set; }
    public virtual User User { get; set; } = null!;
    public Guid ChallengeId { get; set; }
    public virtual Challenge Challenge { get; set; } = null!;   
}