using SalaryAdvanced.Domain.Entities;

namespace SalaryAdvanced.Application.Interfaces
{
    public interface ILimitSalaryRepository
    {
        Task<List<LimitSalary>> GetAllAsync();
        Task<LimitSalary?> GetByIdAsync(int id);
        Task<List<LimitSalary>> SearchAsync(LimitSalary filter);
        Task AddAsync(LimitSalary item);
        Task UpdateAsync(LimitSalary item);
        Task DeleteAsync(int id);
    }
}
