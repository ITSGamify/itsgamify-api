using its.gamify.core.Models.Departments;
using System.Text.Json.Serialization;

namespace its.gamify.core.Models.Users
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        [JsonPropertyName("employee_code")]
        public string EmployeeCode { get; set; } = string.Empty;
        [JsonPropertyName("dept_name")]
        public string DeptName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        [JsonPropertyName("full_name")]
        public string FullName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        [JsonPropertyName("phone_number")]
        public string? PhoneNumber { get; set; } = string.Empty;
        [JsonPropertyName("metric_description")]
        public string MetricDescription { get; set; } = string.Empty;
        [JsonPropertyName("department_id")]
        public Guid DepartmentId { get; set; }
        [JsonPropertyName("role")]
        public string RoleName { get; set; } = string.Empty;
        public DepartmentViewModel Department { get; set; } = new();

    }


}
