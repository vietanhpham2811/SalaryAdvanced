namespace SalaryAdvanced.Application.DTOs
{
    public class CreateRequestDTO
    {
        public int EmployeeId { get; set; } = 0;
        public string RequestCode { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Reason { get; set; } = string.Empty;
    }

    public class RequestResponseDTO
    {
        public int Id { get; set; }
        public int? ApprovedById { get; set; }
        public int StatusId { get; set; }
        public string? RejectionReason { get; set; }
    }

    public class GetRequestDTO
    {
        public int Id { get; set; }
        public string RequestCode { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string? Reason { get; set; }
        public DateTime SubmittedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public int StatusId { get; set; } = 0;
        public int EmployeeId { get; set; } = 0;
        public int? ApprovedById { get; set; }
        public string? RejectionReason { get; set; }
    }
}

//public enum RequestStatus
//{
//    Pending = 1,
//    Approved = 2,
//    Rejected = 3
//}
