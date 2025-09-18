using Microsoft.EntityFrameworkCore;
using SalaryAdvanced.Domain.Entities;
using SalaryAdvanced.Domain.Interfaces;
using SalaryAdvanced.Infrastructure.Data;

namespace SalaryAdvanced.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;
        public DepartmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Department?> AddAsync(Department dept)
        {
            if (dept == null)
                return null;
            await _context.Departments.AddAsync(dept);
            return dept;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);
            if (department == null)
                return false;
            _context.Departments.Remove(department);
            return true;
        }

        public async Task<List<Department>> GetAllAsync()
        {
            return await _context.Departments.AsNoTracking().ToListAsync();
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            return await _context.Departments.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Department?> UpdateAsync(int id, Department dept)
        {
            var existingDepartment = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);
            if (existingDepartment == null)
                return null;
            existingDepartment.Name = dept.Name;
            existingDepartment.Code = dept.Code;
            existingDepartment.Description = dept.Description;
            existingDepartment.ManagerId = dept.ManagerId;
            return existingDepartment;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Departments.AnyAsync(d => d.Id == id);
        }
    }
}
