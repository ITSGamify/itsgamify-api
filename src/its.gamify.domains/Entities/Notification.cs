namespace its.gamify.domains.Entities;

public class Notification : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public int Precedence { get; set; } = 0;
    public bool IsNotified { get; set; } = false;
    public bool IsRead { get; set; } = false;
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = null!;
}