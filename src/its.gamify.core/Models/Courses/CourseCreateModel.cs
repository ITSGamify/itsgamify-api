namespace its.gamify.core.Models.Courses
{
    public class CourseCreateModel
    {
        public string Title { get; set; } = string.Empty;
        public double DurationInHours { get; set; } = 0.0;
        public string Description { get; set; } = string.Empty;
        public Guid QuarterId { get; set; }
        public Guid DifficultyLevelId { get; set; }
        public Guid CategoryId { get; set; }
    }
}
