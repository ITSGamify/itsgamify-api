namespace its.gamify.infras.FluentApis;
public class CourseResultConfiguration : Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<its.gamify.domains.Entities.CourseResult>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<its.gamify.domains.Entities.CourseResult> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        
        builder.HasOne(x => x.Course)
            .WithMany(x => x.CourseResults)
            .HasForeignKey(x => x.CourseId)
            .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.NoAction);
        
        builder.HasOne(x => x.User)
            .WithMany(x => x.CourseResults)
            .HasForeignKey(x => x.UserId)
            .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.NoAction);
    }
}