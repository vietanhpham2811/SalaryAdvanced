using SalaryAdvanced.Application.DTOs;
using SalaryAdvanced.Domain.Entities;

namespace SalaryAdvanced.Application.Interfaces
{
    public interface IDepartmentService
    {
        Task<List<GetDepartmentDTO>> GetAllDepartmentsAsync();
        Task<GetDepartmentDTO?> GetDepartmentByIdAsync(int id);
        Task<GetDepartmentDTO?> AddDepartmentAsync(DepartmentDTO dto);
        Task<GetDepartmentDTO?> UpdateDepartmentAsync(int id, DepartmentDTO dto);
        Task<bool>DepartmentExist(int id);
        Task<bool>DeleteDepartmentAsync(int id);
    }
}
