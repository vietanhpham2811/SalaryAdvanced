using System.ComponentModel.DataAnnotations;

namespace SalaryAdvanced.Domain.Entities
{
    public class Department : BaseEntity
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int? ManagerId { get; set; }

        // Navigation properties
        public ApplicationUser? Manager { get; set; }
        public ICollection<ApplicationUser> Employees { get; set; } = new List<ApplicationUser>();
    }
}