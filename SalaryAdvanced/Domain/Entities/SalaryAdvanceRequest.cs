using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalaryAdvanced.Domain.Entities
{
    public class SalaryAdvanceRequest : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string RequestCode { get; set; } = string.Empty;

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        [Column(TypeName = "decimal(15,2)")]
        public decimal Amount { get; set; }

        public string? Reason { get; set; }

        [Required]
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ProcessedAt { get; set; }

        [Required]
        public int StatusId { get; set; }

        public int? ApprovedById { get; set; }

        public string? RejectionReason { get; set; }

        // Navigation properties
        public ApplicationUser ApplicationUser { get; set; } = null!;
        public RequestStatus Status { get; set; } = null!;
        public ApplicationUser? ApprovedByUser { get; set; }
    }
}