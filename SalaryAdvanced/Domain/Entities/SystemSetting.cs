using System.ComponentModel.DataAnnotations;

namespace SalaryAdvanced.Domain.Entities
{
    public class SystemSetting : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string SettingKey { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string SettingValue { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}