using its.gamify.domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace its.gamify.infras.FluentApis;

public class CourseSectionConfiguration : IEntityTypeConfiguration<CourseSection>
{
    public void Configure(EntityTypeBuilder<CourseSection> builder)
    {
        builder.HasOne(x => x.Course)
        .WithMany(x => x.CourseSections)
        .HasForeignKey(x => x.CourseId)
        .OnDelete(DeleteBehavior.NoAction); // Ensures that if a course is deleted, its sections are also deleted
    }
}