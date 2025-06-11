using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace its.gamify.infras.FluentApis;

public class LessonConfiguration : IEntityTypeConfiguration<domains.Entities.Lesson>
{
    public void Configure(EntityTypeBuilder<domains.Entities.Lesson> builder)
    {
        builder.HasOne(x => x.CourseSection)
            .WithMany(x => x.Lessons)
            .HasForeignKey(x => x.CourseSectionId)
            .OnDelete(DeleteBehavior.NoAction); // Ensures that if a course section is deleted, its lessons are also deleted
        builder.HasMany(x => x.Quizzes)
        .WithOne(x => x.Lesson)
        .HasForeignKey(x => x.LessonId)
            .OnDelete(DeleteBehavior.NoAction); // Ensures that if a lesson is deleted, its quizzes are also deleted
    }
}