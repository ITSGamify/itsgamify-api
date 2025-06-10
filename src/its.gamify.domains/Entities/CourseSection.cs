namespace its.gamify.domains.Entities;

public class CourseSection : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public int OrderedNumber { get; set; }

    public virtual ICollection<LearningMaterial> LearningMaterials { get; set; } = [];
    public virtual ICollection<Quiz> Quizzes { get; set; } = [];
    
}