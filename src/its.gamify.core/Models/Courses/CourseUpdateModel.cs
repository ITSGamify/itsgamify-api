﻿namespace its.gamify.core.Models.Courses
{
    public class CourseUpdateModel : CourseCreateModel
    {
        public Guid Id { get; set; }
        public string? Status { get; set; }
    }
}
