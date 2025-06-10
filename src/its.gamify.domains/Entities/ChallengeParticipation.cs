namespace its.gamify.domains.Entities;

public class ChallengeParticipation : BaseEntity
{

    public string Status { get; set; } = string.Empty;
    public Guid ChallengeId { get; set; }
    public virtual Challenge Challenge { get; set; } = null!;
    public Guid EmployeeId { get; set; }
    public virtual User Employee { get; set; } = null!;
    


}