namespace its.gamify.domains.Entities;

public class WishList : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public Guid CourseId { get; set; }
    public virtual Course Course { get; set; } = null!;
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = null!;
}