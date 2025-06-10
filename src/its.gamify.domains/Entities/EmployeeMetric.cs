namespace its.gamify.domains.Entities;

public class EmployeeMetric : BaseEntity
{
    public string Description { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = null!;
}