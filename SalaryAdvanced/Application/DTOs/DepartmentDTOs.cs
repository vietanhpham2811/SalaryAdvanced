using System.ComponentModel.DataAnnotations;

namespace SalaryAdvanced.Application.DTOs
{
    public class DepartmentDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;
        [MaxLength(500)]
        public string? Description { get; set; }
        public int? ManagerId { get; set; } = 0;
    }
    public class GetDepartmentDTO
    {
        public int Id { get; set; } = 0;
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;
        [MaxLength(500)]
        public string? Description { get; set; }
        public int? ManagerId { get; set; } = 0;
    }
}
