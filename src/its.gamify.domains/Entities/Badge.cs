namespace its.gamify.domains.Entities;

public class Badge : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime ClaimedDate { get; set; }
    #region  RelationshipConfiguration
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = null!;
    #endregion
}