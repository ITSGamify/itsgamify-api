using its.gamify.domains.Enums;

namespace its.gamify.domains.Entities;

public class User : BaseEntity
{
    public string EmpployeeCode { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string? HashedPassword { get; set; } = string.Empty;
    public byte[]? Salt { get; set; } = null;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; } = null;
    public DateTime? DateJoined { get; set; } = null;
    public DateTime? DateOfBirth { get; set; } = null;
    public string Status { get; set; } = UserStatusEnum.Active.ToString();


    #region  Relationship Configuration 
    public Guid RoleId { get; set; }
    public virtual Role? Role { get; set; }
    public virtual EmployeeMetric? EmployeeMetric { get; set; }
    public virtual ICollection<CourseParticipation>? CourseParticipations { get; set; }
    public virtual ICollection<Badge>? Badges { get; set; }
    public virtual ICollection<CourseResult> CourseResults { get; set; } = new List<CourseResult>();
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public Guid DepartmentId { get; set; }
    public virtual Department? Department { get; set; }
    public virtual ICollection<EmployeeChallenge> EmployeeChallenges { get; set; } = new List<EmployeeChallenge>();
    #endregion




}