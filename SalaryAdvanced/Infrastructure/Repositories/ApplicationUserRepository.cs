using Microsoft.EntityFrameworkCore;
using SalaryAdvanced.Domain.Entities;
using SalaryAdvanced.Domain.Interfaces;
using SalaryAdvanced.Infrastructure.Data;

namespace SalaryAdvanced.Infrastructure.Repositories
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<ApplicationUser?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<ApplicationUser?> GetByEmployeeCodeAsync(string employeeCode)
        {
            return await _dbSet
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.EmployeeCode == employeeCode);
        }

        public async Task<IEnumerable<ApplicationUser>> GetByDepartmentAsync(int departmentId)
        {
            return await _dbSet
                .Where(u => u.DepartmentId == departmentId)
                .ToListAsync();
        }

        public async Task<ApplicationUser?> GetWithDepartmentAndRoleAsync(int id)
        {
            return await _dbSet
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> IsEmailUniqueAsync(string email, int? excludeId = null)
        {
            var query = _dbSet.Where(u => u.Email == email);
            if (excludeId.HasValue)
            {
                query = query.Where(u => u.Id != excludeId.Value);
            }
            return !await query.AnyAsync();
        }

        public async Task<bool> IsEmployeeCodeUniqueAsync(string employeeCode, int? excludeId = null)
        {
            var query = _dbSet.Where(u => u.EmployeeCode == employeeCode);
            if (excludeId.HasValue)
            {
                query = query.Where(u => u.Id != excludeId.Value);
            }
            return !await query.AnyAsync();
        }
    }
}