using System.ComponentModel.DataAnnotations;

namespace SalaryAdvanced.Application.DTOs
{
    public class CreateSalaryAdvanceRequestDto
    {
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Số tiền phải lớn hơn 0")]
        public decimal Amount { get; set; }

        [MaxLength(1000)]
        public string? Reason { get; set; }
    }

    public class SalaryAdvanceRequestDto
    {
        public int Id { get; set; }
        public string RequestCode { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string? Reason { get; set; }
        public DateTime SubmittedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? RejectionReason { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string EmployeeCode { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public decimal BasicSalary { get; set; }
        public string? ApproverName { get; set; }
        public decimal PercentageOfSalary { get; set; }
        public int ProcessingHours { get; set; }
    }

    public class EmployeeDto
    {
        public int Id { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public decimal BasicSalary { get; set; }
        public DateTime HireDate { get; set; }
        public bool IsActive { get; set; }      
        public string DepartmentName { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public bool IsDepartmentManager { get; set; }
    }

    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }
}