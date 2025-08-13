namespace its.gamify.core.Models.Users
{
    public class UserCreateModel
    {
        public string EmployeeCode { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public Guid DeptId { get; set; } = Guid.Empty;
        public Guid RoleId { get; set; } = Guid.Empty;

    }
}
