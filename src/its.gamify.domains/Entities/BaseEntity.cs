namespace its.gamify.domains.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime UpdatedDate { get; set; } = DateTime.Now;
    public bool IsDeleted { get; set; } = false;
    public Guid CreatedBy { get; set; } = Guid.Empty;   
    public Guid UpdatedBy { get; set; } = Guid.Empty;
}