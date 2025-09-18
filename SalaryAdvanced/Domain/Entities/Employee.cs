using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalaryAdvanced.Domain.Entities
{
    public class Employee : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string EmployeeCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Phone { get; set; }

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public int RoleId { get; set; }

        [Required]
        [Column(TypeName = "decimal(15,2)")]
        public decimal BasicSalary { get; set; }

        [Required]
        public DateTime HireDate { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public Department Department { get; set; } = null!;
        public Role Role { get; set; } = null!;

        // Department management relationship
        public Department? ManagedDepartment { get; set; }

        // Salary advance requests
        public ICollection<SalaryAdvanceRequest> SalaryAdvanceRequests { get; set; } = new List<SalaryAdvanceRequest>();
        public ICollection<SalaryAdvanceRequest> ApprovedRequests { get; set; } = new List<SalaryAdvanceRequest>();
    }
}