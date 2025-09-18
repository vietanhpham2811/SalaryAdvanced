using System.ComponentModel.DataAnnotations;

namespace SalaryAdvanced.Domain.Entities
{
    public class RequestStatus : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        // Navigation property
        public ICollection<SalaryAdvanceRequest> SalaryAdvanceRequests { get; set; } = new List<SalaryAdvanceRequest>();
    }
}