using SalaryAdvanced.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalaryAdvanced.Domain.Entities
{
    public class Role : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        // Navigation property
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}