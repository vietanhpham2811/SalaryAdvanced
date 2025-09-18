using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalaryAdvanced.Domain.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        [Required]
        [MaxLength(50)]
        public string EmployeeCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Phone { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        [Column(TypeName = "decimal(15,2)")]
        public decimal BasicSalary { get; set; }

        [Required]
        public DateTime HireDate { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Department Department { get; set; } = null!;
        public Department? ManagedDepartment { get; set; }

        // Salary advance requests
        public ICollection<SalaryAdvanceRequest> SalaryAdvanceRequests { get; set; } = new List<SalaryAdvanceRequest>();
        public ICollection<SalaryAdvanceRequest> ApprovedRequests { get; set; } = new List<SalaryAdvanceRequest>();
    }
}