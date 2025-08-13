using its.gamify.domains.Entities;
using its.gamify.domains.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
namespace its.gamify.infras.Datas;

public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    private DbSet<User> user;
    private DbSet<Role> role;

    public AppDbContext(Microsoft.EntityFrameworkCore.DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<FileEntity> File { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<User> User { get => user; set => user = value; }
    public Microsoft.EntityFrameworkCore.DbSet<Role> Role { get => role; set => role = value; }
    public Microsoft.EntityFrameworkCore.DbSet<WishList> WishList { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<QuizResult> QuizResult { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<QuizAnswer> QuizAnswer { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Quiz> Quiz { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Question> Question { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Quarter> Quarter { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<PracticeTag> PracticeTag { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Practice> Practice { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Notification> Notification { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Lesson> Lesson { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<LearningProgress> LearningProgress { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<LearningMaterial> LearningMaterial { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<LeadearBoard> LeadearBoard { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<EmployeeMetric> EmployeeMetric { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Difficulty> Difficulty { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Department> Department { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<CourseSection> CourseSection { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<CourseReview> CourseReview { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<CourseResult> CourseResult { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<CourseParticipation> CourseParticipation { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Course> Course { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<ChallengeParticipation> ChallengeParticipation { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Challenge> Challenge { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Category> Category { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Badge> Badge { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.ConfigureWarnings(warnings =>
     warnings.Ignore(RelationalEventId.PendingModelChangesWarning));


    }
    protected override void OnModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(its.gamify.infras.AssemblyReference.Assembly);
        Role[] roles = [new Role()
        {
            Id = Guid.Parse("71874fd3-1892-4d92-a77f-c85c0d16b8db"),
            Name = RoleEnum.EMPLOYEE.ToString()
        }, new Role()
        {
                Id = Guid.Parse("620d170e-c32e-4443-b450-32848c1eb5e9"),
            Name = RoleEnum.LEADER.ToString()
        }, new Role() {
                Id = Guid.Parse("3b72db68-b2c6-40d8-859e-b4996f8535a1"),
            Name = RoleEnum.TRAININGSTAFF.ToString()}, new Role()
        {
                    Id = Guid.Parse("f7fa7c6b-f76a-4b95-8711-517eb8205a1a"),
            Name = RoleEnum.MANAGER.ToString()
        }, new Role()
        {     Id = Guid.Parse("b002d347-66b9-4722-9547-5b2165abaa9f"),
            Name = RoleEnum.ADMIN.ToString()}];


        modelBuilder.Entity<Role>().HasData(roles);
    }
}