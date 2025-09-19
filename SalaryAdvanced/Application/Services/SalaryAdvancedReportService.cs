using Microsoft.EntityFrameworkCore;
using SalaryAdvanced.Application.DTOs;
using SalaryAdvanced.Application.Interfaces;
using SalaryAdvanced.Domain.Interfaces;
using ClosedXML.Excel;
using System.IO;
using Microsoft.Extensions.Logging;

namespace SalaryAdvanced.Application.Services
{
    public class SalaryAdvanceReportService : ISalaryAdvancedReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticationService _authService;
        private readonly ILogger<SalaryAdvanceReportService> _logger;

        public SalaryAdvanceReportService(
            IUnitOfWork unitOfWork,
            IAuthenticationService authService,
            ILogger<SalaryAdvanceReportService> logger)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _logger = logger;
        }

        public async Task<List<SalaryAdvanceReportDto>> GetReportAsync(ReportFilterDto filter)
        {
            try
            {
                var currentUser = await _authService.GetCurrentUserAsync();
                if (currentUser == null)
                    return new List<SalaryAdvanceReportDto>();

                var allRequests = await _unitOfWork.SalaryAdvanceRequests.GetAllWithIncludesAsync();
                var requestsList = allRequests.ToList();

                if (filter.FromDate.HasValue)
                    requestsList = requestsList.Where(r => r.SubmittedAt >= filter.FromDate.Value).ToList();
                if (filter.ToDate.HasValue)
                    requestsList = requestsList.Where(r => r.SubmittedAt <= filter.ToDate.Value).ToList();

                var isManager = await _authService.IsInRoleAsync(currentUser, "Manager");
                if (isManager)
                {
                    var managedDepartment = await _unitOfWork.Departments
                        .SingleOrDefaultAsync(d => d.ManagerId == currentUser.Id);
                    if (managedDepartment != null)
                    {
                        requestsList = requestsList.Where(r => r.ApplicationUser.DepartmentId == managedDepartment.Id).ToList();
                    }
                    else
                    {
                        return new List<SalaryAdvanceReportDto>();
                    }
                }

                if (!string.IsNullOrEmpty(filter.Status))
                    requestsList = requestsList.Where(r => r.Status?.Name == filter.Status).ToList();
                if (!string.IsNullOrEmpty(filter.EmployeeName))
                    requestsList = requestsList.Where(r => r.ApplicationUser?.FullName?.ToLower().Contains(filter.EmployeeName.ToLower()) == true).ToList();
                if (!string.IsNullOrEmpty(filter.EmployeeCode))
                    requestsList = requestsList.Where(r => r.ApplicationUser?.EmployeeCode?.ToLower().Contains(filter.EmployeeCode.ToLower()) == true).ToList();

                var requests = requestsList.Select(r => new SalaryAdvanceReportDto
                {
                    Id = r.Id,
                    EmployeeCode = r.ApplicationUser?.EmployeeCode ?? "",
                    EmployeeName = r.ApplicationUser?.FullName ?? "",
                    DepartmentName = r.ApplicationUser?.Department?.Name ?? "",
                    RequestAmount = r.Amount,
                    RequestDate = r.CreatedAt,
                    ApprovalDate = r.SubmittedAt,
                    Status = r.Status?.Name ?? "",
                    RejectReason = r.RejectionReason,
                    Reason = r.Reason ?? "",
                    ApprovedByName = r.ApprovedByUser?.FullName
                })
                .OrderByDescending(r => r.RequestDate)
                .ToList();
                return requests;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting salary advance report with filter: {@Filter}", filter);
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<ReportSummaryDto> GetReportSummaryAsync(ReportFilterDto filter)
        {
            try
            {
                var requests = await GetReportAsync(filter);
                var summary = new ReportSummaryDto
                {
                    TotalRequests = requests.Count,
                    PendingRequests = requests.Count(r => r.Status == "Pending"),
                    ApprovedRequests = requests.Count(r => r.Status == "Approved"),
                    RejectedRequests = requests.Count(r => r.Status == "Rejected"),
                    TotalRequestAmount = requests.Sum(r => r.RequestAmount),
                    TotalApprovedAmount = requests.Where(r => r.Status == "Approved").Sum(r => r.RequestAmount),
                    TotalPendingAmount = requests.Where(r => r.Status == "Pending").Sum(r => r.RequestAmount),
                    AverageProcessingDays = requests.Where(r => r.ApprovalDate.HasValue)
                        .Select(r => (r.ApprovalDate!.Value - r.RequestDate).TotalDays)
                        .DefaultIfEmpty(0)
                        .Average()
                };
                return summary;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting report summary with filter: {@Filter}", filter);
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<byte[]> ExportToExcelAsync(ReportFilterDto filter)
        {
            try
            {
                var requests = await GetReportAsync(filter);
                var summary = await GetReportSummaryAsync(filter);

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Salary Advance Report");

                worksheet.Cell(1, 1).Value = "SALARY ADVANCE REQUEST REPORT";
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(1, 1).Style.Font.FontSize = 16;
                worksheet.Range(1, 1, 1, 12).Merge();
                worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(2, 1).Value = $"Export Date: {DateTime.Now:dd/MM/yyyy HH:mm}";
                worksheet.Cell(3, 1).Value = $"Filter: {GetFilterDescription(filter)}";

                var summaryRow = 5;
                worksheet.Cell(summaryRow, 1).Value = "SUMMARY";
                worksheet.Cell(summaryRow, 1).Style.Font.Bold = true;
                worksheet.Cell(summaryRow, 1).Style.Font.FontSize = 14;
                worksheet.Cell(summaryRow + 1, 1).Value = "Total Requests:";
                worksheet.Cell(summaryRow + 1, 2).Value = summary.TotalRequests;
                worksheet.Cell(summaryRow + 1, 4).Value = "Approved:";
                worksheet.Cell(summaryRow + 1, 5).Value = summary.ApprovedRequests;
                worksheet.Cell(summaryRow + 1, 7).Value = "Pending:";
                worksheet.Cell(summaryRow + 1, 8).Value = summary.PendingRequests;
                worksheet.Cell(summaryRow + 1, 10).Value = "Rejected:";
                worksheet.Cell(summaryRow + 1, 11).Value = summary.RejectedRequests;
                worksheet.Cell(summaryRow + 2, 1).Value = "Total Amount:";
                worksheet.Cell(summaryRow + 2, 2).Value = summary.TotalRequestAmount.ToString("N0") + " VNĐ";
                worksheet.Cell(summaryRow + 2, 4).Value = "Approved:";
                worksheet.Cell(summaryRow + 2, 5).Value = summary.TotalApprovedAmount.ToString("N0") + " VNĐ";
                worksheet.Cell(summaryRow + 2, 7).Value = "Pending:";
                worksheet.Cell(summaryRow + 2, 8).Value = summary.TotalPendingAmount.ToString("N0") + " VNĐ";

                var headerRow = summaryRow + 4;
                var headers = new string[]
                {
                "No", "Employee Code", "Employee Name", "Department", "Amount", "Reason",
                "Request Date", "Status", "Approval Date", "Approved By", "Notes"
                };
                for (int i = 0; i < headers.Length; i++)
                {
                    var cell = worksheet.Cell(headerRow, i + 1);
                    cell.Value = headers[i];
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = XLColor.LightBlue;
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                }
                for (int i = 0; i < requests.Count; i++)
                {
                    var request = requests[i];
                    var row = headerRow + 1 + i;

                    worksheet.Cell(row, 1).Value = i + 1;
                    worksheet.Cell(row, 2).Value = request.EmployeeCode;
                    worksheet.Cell(row, 3).Value = request.EmployeeName;
                    worksheet.Cell(row, 4).Value = request.DepartmentName;
                    worksheet.Cell(row, 5).Value = request.RequestAmount.ToString("N0");
                    worksheet.Cell(row, 6).Value = request.Reason;
                    worksheet.Cell(row, 7).Value = request.RequestDate.ToString("dd/MM/yyyy");
                    worksheet.Cell(row, 8).Value = GetStatusText(request.Status);
                    worksheet.Cell(row, 9).Value = request.ApprovalDate?.ToString("dd/MM/yyyy") ?? "";
                    worksheet.Cell(row, 10).Value = request.ApprovedByName ?? "";
                    worksheet.Cell(row, 11).Value = request.RejectReason ?? "";

                    var statusCell = worksheet.Cell(row, 8);
                    switch (request.Status)
                    {
                        case "Approved":
                            statusCell.Style.Fill.BackgroundColor = XLColor.LightGreen;
                            break;
                        case "Rejected":
                            statusCell.Style.Fill.BackgroundColor = XLColor.LightCoral;
                            break;
                        case "Pending":
                            statusCell.Style.Fill.BackgroundColor = XLColor.LightYellow;
                            break;
                    }

                    for (int j = 1; j <= headers.Length; j++)
                    {
                        worksheet.Cell(row, j).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }
                }

                worksheet.Columns().AdjustToContents();
                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                return stream.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting to Excel with filter: {@Filter}", filter);
                throw new Exception(ex.Message, ex);
            }
        }

        private string GetFilterDescription(ReportFilterDto filter)
        {
            try
            {
                var filters = new List<string>();

                if (filter.FromDate.HasValue)
                    filters.Add($"From {filter.FromDate.Value:dd/MM/yyyy}");
                if (filter.ToDate.HasValue)
                    filters.Add($"To {filter.ToDate.Value:dd/MM/yyyy}");
                if (!string.IsNullOrEmpty(filter.Status))
                    filters.Add($"Status: {GetStatusText(filter.Status)}");
                if (!string.IsNullOrEmpty(filter.EmployeeName))
                    filters.Add($"Employee: {filter.EmployeeName}");
                return filters.Count > 0 ? string.Join(", ", filters) : "All";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting filter description");
                return "Filter error";
            }
        }

        private string GetStatusText(string status)
        {
            try
            {
                return status switch
                {
                    "Pending" => "Pending",
                    "Approved" => "Approved",
                    "Rejected" => "Rejected",
                    _ => status
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting status text for status: {Status}", status);
                return status ?? "Unknown";
            }
        }
    }
}