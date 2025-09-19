using System.ComponentModel.DataAnnotations;

namespace SalaryAdvanced.Application.DTOs
{
    public class SalaryAdvanceReportDto
    {
        public int Id { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public decimal RequestAmount { get; set; }
        public string Reason { get; set; } = string.Empty;
        public DateTime RequestDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? ApprovalNotes { get; set; }
        public string? ApprovedByName { get; set; }
    }

    public class ReportFilterDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Status { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeCode { get; set; }
    }

    public class ReportSummaryDto
    {
        public int TotalRequests { get; set; }
        public int PendingRequests { get; set; }
        public int ApprovedRequests { get; set; }
        public int RejectedRequests { get; set; }
        public decimal TotalRequestAmount { get; set; }
        public decimal TotalApprovedAmount { get; set; }
        public decimal TotalPendingAmount { get; set; }
        public double AverageProcessingDays { get; set; }
    }
}
