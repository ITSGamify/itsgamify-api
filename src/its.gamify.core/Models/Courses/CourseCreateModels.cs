using its.gamify.core.Models.CourseSections;
using its.gamify.domains.Enums;
using System.Text.Json.Serialization;

namespace its.gamify.core.Models.Courses
{
    public class CourseCreateModels
    {

        public string Title { get; set; } = string.Empty;
        [JsonPropertyName("classify")]
        public CourseTypeEnum CourseType { get; set; } = CourseTypeEnum.ALL;
        [JsonPropertyName("short_description")]
        public string Description { get; set; } = string.Empty;
        [JsonPropertyName("description")]
        public string LongDescription { get; set; } = string.Empty;
        [JsonPropertyName("thumbnail_image_id")]
        public Guid ThumbNailImageId { get; set; } = Guid.Empty;
        [JsonPropertyName("introduction_video_id")]
        public Guid IntroductionVideoId { get; set; } = Guid.Empty;
        public List<string> Tags { get; set; } = new();

        public List<string> Targets { get; set; } = [];
        [JsonPropertyName("requirement")]
        public string Requirements { get; set; } = string.Empty;
        [JsonPropertyName("department_id")]
        public Guid DepartmentId { get; set; }
        [JsonPropertyName("category_id")]
        public Guid CategoryId { get; set; }
        [JsonPropertyName("modules")]
        public List<CourseSectionCreateModel>? CourseSectionCreate { get; set; }

        [JsonPropertyName("file_ids")]
        public List<Guid> LearningMaterialIds { get; set; } = [];
    }
}
