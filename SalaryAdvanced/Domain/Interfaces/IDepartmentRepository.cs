using SalaryAdvanced.Domain.Entities;

namespace SalaryAdvanced.Domain.Interfaces
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        //Task<List<Department>> GetAllAsync();
        //Task<Department?> GetByIdAsync(int id);
        //Task<Department?> AddAsync(Department dept);
        Task<Department?> UpdateAsync(int id, Department dept);
        Task<bool> DeleteAsync(int id);
        //Task<bool> ExistsAsync(int id);
    }
}
