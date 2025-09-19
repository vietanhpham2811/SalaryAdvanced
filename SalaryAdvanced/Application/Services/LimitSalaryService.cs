using Microsoft.EntityFrameworkCore;
using SalaryAdvanced.Application.DTOs;
using SalaryAdvanced.Application.Interfaces;
using SalaryAdvanced.Domain.Entities;
using SalaryAdvanced.Infrastructure.Data;

namespace SalaryAdvanced.Application.Services
{
    public class LimitSalaryService : ILimitSalaryRepository
    {
        private readonly ApplicationDbContext _context;

        public LimitSalaryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<LimitSalary>> GetAllAsync()
        {
            return await _context.LimitSalarys.OrderByDescending(x=>x.last_change).ToListAsync();
        }
        public async Task<List<DepartmentDto>> GetDepartmentsAsync()
        {
            return await _context.Departments
                .Select(d => new DepartmentDto
                {
                    Id = d.Id,
                    Name = d.Name
                })
                .ToListAsync();
        }
        public async Task<LimitSalary?> GetByIdAsync(int id)
        {
            return await _context.LimitSalarys.FindAsync(id);
        }

        public async Task<List<LimitSalary>> SearchAsync(LimitSalary filter)
        {
            IQueryable<LimitSalary> query = _context.LimitSalarys;

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                query = query.Where(x => x.Name.Contains(filter.Name));
            }
            if (!string.IsNullOrWhiteSpace(filter.Type))
            {
                query = query.Where(x => x.Type.Contains(filter.Type));
            }
            if (filter.ObjectId.HasValue)
            {
                query = query.Where(x => x.ObjectId == filter.ObjectId);
            }

            return await query.OrderByDescending(x=>x.create_date).ToListAsync();
        }

        public async Task AddAsync(LimitSalary item)
        {
            _context.LimitSalarys.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LimitSalary item)
        {
            var existing = await _context.LimitSalarys.FindAsync(item.Id);
            if (existing != null)
            {
                existing.Name = item.Name;
                existing.Type = item.Type;
                existing.ObjectId = item.ObjectId;
                existing.MaxOncePercent = item.MaxOncePercent;
                existing.MaxTimesPerMonth = item.MaxTimesPerMonth;
                existing.MaxMonthlyPercent = item.MaxMonthlyPercent;
                existing.ValidFromDay = item.ValidFromDay;
                existing.ValidToDay = item.ValidToDay;
                existing.last_change = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }


        public async Task DeleteAsync(int id)
        {
            var entity = await _context.LimitSalarys.FindAsync(id);
            if (entity != null)
            {
                _context.LimitSalarys.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }

}
