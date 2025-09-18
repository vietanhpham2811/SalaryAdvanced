using SalaryAdvanced.Domain.Entities;

namespace SalaryAdvanced.Domain.Interfaces
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        Task<ApplicationUser?> GetByEmailAsync(string email);
        Task<ApplicationUser?> GetByEmployeeCodeAsync(string employeeCode);
        Task<IEnumerable<ApplicationUser>> GetByDepartmentAsync(int departmentId);
        Task<ApplicationUser?> GetWithDepartmentAndRoleAsync(int id);
        Task<bool> IsEmailUniqueAsync(string email, int? excludeId = null);
        Task<bool> IsEmployeeCodeUniqueAsync(string employeeCode, int? excludeId = null);
    }
}