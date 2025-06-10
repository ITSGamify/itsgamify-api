namespace its.gamify.domains.Entities;

public class Quarter : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int Year { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public virtual ICollection<Course>? Courses { get; set; }
}