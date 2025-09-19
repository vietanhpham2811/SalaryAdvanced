using Microsoft.EntityFrameworkCore;
using SalaryAdvanced.Domain.Entities;
using SalaryAdvanced.Domain.Interfaces;
using SalaryAdvanced.Infrastructure.Data;

namespace SalaryAdvanced.Infrastructure.Repositories
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(ApplicationDbContext context): base(context)
        {
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var department = await _dbSet.FirstOrDefaultAsync(d => d.Id == id);
            if (department == null)
                return false;
            _dbSet.Remove(department);
            return true;
        }
        public async Task<Department?> UpdateAsync(int id, Department dept)
        {
            var existingDepartment = await _dbSet.FirstOrDefaultAsync(d => d.Id == id);
            if (existingDepartment == null)
                return null;
            existingDepartment.Name = dept.Name;
            existingDepartment.Code = dept.Code;
            existingDepartment.Description = dept.Description;
            existingDepartment.ManagerId = dept.ManagerId;
            return existingDepartment;
        }
    }
}
