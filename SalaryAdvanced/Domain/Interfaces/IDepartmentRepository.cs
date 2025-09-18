using SalaryAdvanced.Domain.Entities;

namespace SalaryAdvanced.Domain.Interfaces
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        Task<Department?> GetByCodeAsync(string code);
        Task<Department?> GetWithManagerAsync(int id);
        Task<IEnumerable<Department>> GetAllWithManagersAsync();
    }
}