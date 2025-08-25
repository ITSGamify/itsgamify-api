namespace its.gamify.domains.Entities;

public class Notification : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public Guid TargetEntity { get; set; }
    public bool IsRead { get; set; } = false;
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = null!;
}