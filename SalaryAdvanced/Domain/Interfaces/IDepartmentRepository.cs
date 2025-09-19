using SalaryAdvanced.Domain.Entities;

namespace SalaryAdvanced.Domain.Interfaces
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        Task<Department?> UpdateAsync(int id, Department dept);
        Task<bool> DeleteAsync(int id);
    }
}
