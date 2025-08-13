using its.gamify.domains.Enums;
using System.Text.Json.Serialization;

namespace its.gamify.domains.Entities;

public class Course : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    [JsonPropertyName("duration_in_hours")]
    public double DurationInHours { get; set; } = 0.0;
    [JsonPropertyName("short_description")]
    public string Description { get; set; } = string.Empty;
    [JsonPropertyName("description")]
    public string LongDescription { get; set; } = string.Empty;
    [JsonPropertyName("classify")]
    public string CourseType { get; set; } = CourseTypeEnum.ALL.ToString();
    [JsonPropertyName("tags")]
    public List<string> Tags { get; set; } = [];
    [JsonPropertyName("targets")]
    public List<string> Targets { get; set; } = [];
    [JsonPropertyName("requirement")]
    public string Requirements { get; set; } = string.Empty;
    [JsonPropertyName("introduction_video")]
    public string IntroVideo { get; set; }

    [JsonPropertyName("thumbnail_image")]
    public string ThumbnailImage { get; set; }

    [JsonPropertyName("thumbnail_image_id")]
    public Guid ThumbnailId { get; set; } = Guid.Empty;
    [JsonPropertyName("intro_video_id")]
    public Guid IntroVideoId { get; set; } = Guid.Empty;
    [JsonPropertyName("drafted")]
    public bool IsDraft { get; set; } = false;
    public string Status { get; set; } = CourseStatusEnum.INITIAL.ToString();

    public virtual ICollection<CourseResult> CourseResults { get; set; } = [];
    public virtual ICollection<CourseParticipation> CourseParticipations { get; set; } = [];
    public virtual ICollection<CourseReview> CourseReviews { get; set; } = [];
    [JsonPropertyName("modules")]
    public virtual ICollection<CourseSection> CourseSections { get; set; } = [];
    public virtual ICollection<Practice> Practices { get; set; } = [];
    [JsonPropertyName("learning_materials")]
    public virtual ICollection<LearningMaterial> LearningMaterials { get; set; } = [];
    public Guid QuarterId { get; set; }
    public virtual Quarter Quarter { get; set; } = null!;


    public virtual ICollection<WishList> WishLists { get; set; } = [];
    public Guid CategoryId { get; set; }
    public virtual Category Category { get; set; } = null!;

}