namespace its.gamify.domains.Entities;

public class Role : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;


    #region Relationship Configuration 
    public virtual ICollection<User>? Users { get; set; }
    #endregion;
}