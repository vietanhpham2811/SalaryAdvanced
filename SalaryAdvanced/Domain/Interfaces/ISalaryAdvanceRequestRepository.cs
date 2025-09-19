using SalaryAdvanced.Domain.Entities;

namespace SalaryAdvanced.Domain.Interfaces
{
    public interface ISalaryAdvanceRequestRepository : IRepository<SalaryAdvanceRequest>
    {
        Task<IEnumerable<SalaryAdvanceRequest>> GetByEmployeeAsync(int employeeId);
        Task<IEnumerable<SalaryAdvanceRequest>> GetPendingRequestsAsync();
        Task<IEnumerable<SalaryAdvanceRequest>> GetRequestsForApprovalAsync(int managerId);
        Task<IEnumerable<SalaryAdvanceRequest>> GetByEmployeeAndMonthAsync(int employeeId, DateTime month);
        Task<decimal> GetTotalApprovedAmountByEmployeeAndMonthAsync(int employeeId, DateTime month);
        Task<int> GetRequestCountByEmployeeAndMonthAsync(int employeeId, DateTime month);
        Task<SalaryAdvanceRequest?> GetWithDetailsAsync(int id);
        Task<IEnumerable<SalaryAdvanceRequest>> GetRequestHistoryAsync(int employeeId);
        Task<SalaryAdvanceRequest?> ResponseRequestAsync(SalaryAdvanceRequest response);
        Task<bool> DeleteRequest(int id);
        Task<IEnumerable<SalaryAdvanceRequest>> GetByStatusAsync(int statusId);
    }
}