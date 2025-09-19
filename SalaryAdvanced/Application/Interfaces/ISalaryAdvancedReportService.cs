using SalaryAdvanced.Application.DTOs;

namespace SalaryAdvanced.Application.Interfaces
{
    public interface ISalaryAdvancedReportService
    {
        Task<List<SalaryAdvanceReportDto>> GetReportAsync(ReportFilterDto filter);
        Task<ReportSummaryDto> GetReportSummaryAsync(ReportFilterDto filter);
        Task<byte[]> ExportToExcelAsync(ReportFilterDto filter);
    }
}
