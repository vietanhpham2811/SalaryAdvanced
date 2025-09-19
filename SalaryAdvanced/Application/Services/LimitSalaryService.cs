using Microsoft.EntityFrameworkCore;
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
            return await _context.LimitSalarys.ToListAsync();
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
            if (filter.ObjectId != 0)
            {
                query = query.Where(x => x.ObjectId == filter.ObjectId);
            }

            return await query.ToListAsync();
        }

        public async Task AddAsync(LimitSalary item)
        {
            _context.LimitSalarys.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LimitSalary item)
        {
            _context.LimitSalarys.Update(item);
            await _context.SaveChangesAsync();
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
