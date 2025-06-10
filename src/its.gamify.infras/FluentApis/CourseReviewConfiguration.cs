namespace its.gamify.infras.FluentApis;
public class CourseReviewConfiguration : Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<its.gamify.domains.Entities.CourseReview>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<its.gamify.domains.Entities.CourseReview> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.HasOne(x => x.Course)
            .WithMany(x => x.CourseReviews)
            .HasForeignKey(x => x.CourseId)
            .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.NoAction);


    }
}   