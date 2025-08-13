using its.gamify.domains.Enums;
using System.Text.Json.Serialization;

namespace its.gamify.domains.Entities;

public class Lesson : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Content { get; set; }
    [JsonPropertyName("duration")]
    public int DurationInMinutes { get; set; }
    public string Type { get; set; } = LearningMaterialType.VIDEO.ToString(); // e.g., Video, Article, Quiz
    [JsonPropertyName("video_url")]
    public string? Url { get; set; } = string.Empty; // Link to the lesson material

    public ICollection<Practice> Practices { get; set; } = new List<Practice>();
    public Guid? LearningProgressId { get; set; }
    public virtual LearningProgress? LearningProgress { get; set; } = null!; // Navigation property to the learning progress this lesson belongs to
    public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
    public Guid? CourseSectionId { get; set; }
    public virtual CourseSection CourseSection { get; set; } = null!; // Navigation property to the course section this lesson belongs to
}