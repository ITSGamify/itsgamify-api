using Microsoft.EntityFrameworkCore;

namespace its.gamify.infras.FluentApis;
public class RoleConfiugration : IEntityTypeConfiguration<its.gamify.domains.Entities.Role>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<its.gamify.domains.Entities.Role> builder)
    {
        builder.HasMany(x => x.Users)
            .WithOne(x => x.Role)
            .HasForeignKey(x => x.RoleId)
            .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Cascade);
    }
}