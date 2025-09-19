using Microsoft.EntityFrameworkCore;
using SalaryAdvanced.Application.DTOs;
using SalaryAdvanced.Application.Interfaces;
using SalaryAdvanced.Domain.Interfaces;
using ClosedXML.Excel;
using System.IO;

namespace SalaryAdvanced.Application.Services
{
    public class SalaryAdvanceReportService : ISalaryAdvancedReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticationService _authService;

        public SalaryAdvanceReportService(
            IUnitOfWork unitOfWork,
            IAuthenticationService authService)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public async Task<List<SalaryAdvanceReportDto>> GetReportAsync(ReportFilterDto filter)
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
                Reason = r.Reason ?? "",
                RequestDate = r.SubmittedAt,
                ApprovalDate = r.ProcessedAt,
                Status = r.Status?.Name ?? "",
                ApprovalNotes = r.RejectionReason,
                ApprovedByName = r.ApprovedByUser?.FullName
            })
            .OrderByDescending(r => r.RequestDate)
            .ToList();
            return requests;
        }

        public async Task<ReportSummaryDto> GetReportSummaryAsync(ReportFilterDto filter)
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

        public async Task<byte[]> ExportToExcelAsync(ReportFilterDto filter)
        {
            var requests = await GetReportAsync(filter);
            var summary = await GetReportSummaryAsync(filter);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Báo cáo yêu cầu tạm ứng");

            worksheet.Cell(1, 1).Value = "BÁO CÁO YÊU CẦU TẠM ỨNG LƯƠNG";
            worksheet.Cell(1, 1).Style.Font.Bold = true;
            worksheet.Cell(1, 1).Style.Font.FontSize = 16;
            worksheet.Range(1, 1, 1, 12).Merge();
            worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Cell(2, 1).Value = $"Xuất ngày: {DateTime.Now:dd/MM/yyyy HH:mm}";
            worksheet.Cell(3, 1).Value = $"Bộ lọc: {GetFilterDescription(filter)}";

            var summaryRow = 5;
            worksheet.Cell(summaryRow, 1).Value = "TỔNG QUAN";
            worksheet.Cell(summaryRow, 1).Style.Font.Bold = true;
            worksheet.Cell(summaryRow, 1).Style.Font.FontSize = 14;
            worksheet.Cell(summaryRow + 1, 1).Value = "Tổng số yêu cầu:";
            worksheet.Cell(summaryRow + 1, 2).Value = summary.TotalRequests;
            worksheet.Cell(summaryRow + 1, 4).Value = "Đã phê duyệt:";
            worksheet.Cell(summaryRow + 1, 5).Value = summary.ApprovedRequests;
            worksheet.Cell(summaryRow + 1, 7).Value = "Chờ duyệt:";
            worksheet.Cell(summaryRow + 1, 8).Value = summary.PendingRequests;
            worksheet.Cell(summaryRow + 1, 10).Value = "Từ chối:";
            worksheet.Cell(summaryRow + 1, 11).Value = summary.RejectedRequests;
            worksheet.Cell(summaryRow + 2, 1).Value = "Tổng tiền yêu cầu:";
            worksheet.Cell(summaryRow + 2, 2).Value = summary.TotalRequestAmount.ToString("N0") + " VNĐ";
            worksheet.Cell(summaryRow + 2, 4).Value = "Đã phê duyệt:";
            worksheet.Cell(summaryRow + 2, 5).Value = summary.TotalApprovedAmount.ToString("N0") + " VNĐ";
            worksheet.Cell(summaryRow + 2, 7).Value = "Chờ duyệt:";
            worksheet.Cell(summaryRow + 2, 8).Value = summary.TotalPendingAmount.ToString("N0") + " VNĐ";

            var headerRow = summaryRow + 4;
            var headers = new string[]
            {
                "STT", "Mã NV", "Tên nhân viên", "Phòng ban", "Số tiền", "Lý do",
                "Ngày yêu cầu", "Trạng thái", "Ngày duyệt", "Người duyệt", "Ghi chú"
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
                worksheet.Cell(row, 11).Value = request.ApprovalNotes ?? "";

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

        private string GetFilterDescription(ReportFilterDto filter)
        {
            var filters = new List<string>();

            if (filter.FromDate.HasValue)
                filters.Add($"Từ {filter.FromDate.Value:dd/MM/yyyy}");
            if (filter.ToDate.HasValue)
                filters.Add($"Đến {filter.ToDate.Value:dd/MM/yyyy}");
            if (!string.IsNullOrEmpty(filter.Status))
                filters.Add($"Trạng thái: {GetStatusText(filter.Status)}");
            if (!string.IsNullOrEmpty(filter.EmployeeName))
                filters.Add($"Nhân viên: {filter.EmployeeName}");
            return filters.Count > 0 ? string.Join(", ", filters) : "Tất cả";
        }

        private string GetStatusText(string status)
        {
            return status switch
            {
                "Pending" => "Chờ duyệt",
                "Approved" => "Đã duyệt",
                "Rejected" => "Từ chối",
                _ => status
            };
        }
    }
}