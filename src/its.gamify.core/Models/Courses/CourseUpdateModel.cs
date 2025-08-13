using System.Text.Json.Serialization;

namespace its.gamify.core.Models.Courses
{
    public class CourseUpdateModel : CourseCreateModels
    {
        public Guid Id { get; set; }
        [JsonPropertyName("current_step")]
        public string? Status { get; set; }
        [JsonPropertyName("drafted")]
        public bool? IsDraft { get; set; } = false;
    }
}
