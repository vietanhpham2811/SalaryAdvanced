using Microsoft.EntityFrameworkCore;
using SalaryAdvanced.Domain.Entities;
using SalaryAdvanced.Domain.Interfaces;
using SalaryAdvanced.Infrastructure.Data;

namespace SalaryAdvanced.Infrastructure.Repositories
{
    public class SalaryAdvanceRequestRepository : Repository<SalaryAdvanceRequest>, ISalaryAdvanceRequestRepository
    {
        public SalaryAdvanceRequestRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<SalaryAdvanceRequest>> GetByEmployeeAsync(int employeeId)
        {
            return await _dbSet
                .Include(r => r.Status)
                .Include(r => r.ApprovedByUser)
                .Where(r => r.EmployeeId == employeeId)
                .OrderByDescending(r => r.SubmittedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<SalaryAdvanceRequest>> GetPendingRequestsAsync()
        {
            return await _dbSet
                .Include(r => r.ApplicationUser)
                    .ThenInclude(u => u.Department)
                .Include(r => r.Status)
                .Where(r => r.Status.Name == "Pending")
                .OrderBy(r => r.SubmittedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<SalaryAdvanceRequest>> GetRequestsForApprovalAsync(int managerId)
        {
            return await _dbSet
                .Include(r => r.ApplicationUser)
                    .ThenInclude(u => u.Department)
                .Include(r => r.Status)
                .Where(r => r.Status.Name == "Pending" && r.ApplicationUser.Department.ManagerId == managerId)
                .OrderBy(r => r.SubmittedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<SalaryAdvanceRequest>> GetByEmployeeAndMonthAsync(int employeeId, DateTime month)
        {
            var startOfMonth = new DateTime(month.Year, month.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            return await _dbSet
                .Include(r => r.Status)
                .Where(r => r.EmployeeId == employeeId &&
                           r.SubmittedAt >= startOfMonth &&
                           r.SubmittedAt <= endOfMonth)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalApprovedAmountByEmployeeAndMonthAsync(int employeeId, DateTime month)
        {
            var startOfMonth = new DateTime(month.Year, month.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            return await _dbSet
                .Include(r => r.Status)
                .Where(r => r.EmployeeId == employeeId &&
                           r.Status.Name == "Approved" &&
                           r.SubmittedAt >= startOfMonth &&
                           r.SubmittedAt <= endOfMonth)
                .SumAsync(r => r.Amount);
        }

        public async Task<int> GetRequestCountByEmployeeAndMonthAsync(int employeeId, DateTime month)
        {
            var startOfMonth = new DateTime(month.Year, month.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            return await _dbSet
                .Where(r => r.EmployeeId == employeeId &&
                           r.SubmittedAt >= startOfMonth &&
                           r.SubmittedAt <= endOfMonth)
                .CountAsync();
        }

        public async Task<SalaryAdvanceRequest?> GetWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(r => r.ApplicationUser)
                    .ThenInclude(u => u.Department)
                .Include(r => r.Status)
                .Include(r => r.ApprovedByUser)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<SalaryAdvanceRequest>> GetRequestHistoryAsync(int employeeId)
        {
            return await _dbSet
                .Include(r => r.Status)
                .Include(r => r.ApprovedByUser)
                .Where(r => r.EmployeeId == employeeId)
                .OrderByDescending(r => r.SubmittedAt)
                .ToListAsync();
        }
    }
}