using its.gamify.domains.Enums;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace its.gamify.domains.Entities;

public class User : BaseEntity
{
    [JsonProperty("employee_code")]
    public string EmpployeeCode { get; set; } = string.Empty;
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;
    [JsonPropertyName("hashed_password")]
    public string? HashedPassword { get; set; } = string.Empty;
    [JsonPropertyName("salt")]
    public byte[]? Salt { get; set; } = null;
    public string Email { get; set; } = string.Empty;
    [JsonPropertyName("first_name")]
    public string FirstName { get; set; } = string.Empty;
    [JsonPropertyName("last_name")]
    public string LastName { get; set; } = string.Empty;
    [JsonPropertyName("phone_number")]
    public string? PhoneNumber { get; set; } = null;
    [JsonPropertyName("date_joined")]
    public DateTime? DateJoined { get; set; } = null;
    [JsonPropertyName("date_of_birth")]
    public DateTime? DateOfBirth { get; set; } = null;
    public string Status { get; set; } = UserStatusEnum.ACTIVE.ToString();
    public string? AvatarUrl { get; set; }


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

    #endregion




}