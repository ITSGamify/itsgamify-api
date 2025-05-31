namespace its.gamify.infras.Datas;
public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public AppDbContext(Microsoft.EntityFrameworkCore.DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(its.gamify.core.AssemblyReference.Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(its.gamify.infras.AssemblyReference.Assembly);
        base.OnModelCreating(modelBuilder);
    }
}