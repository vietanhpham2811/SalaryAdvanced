using SalaryAdvanced.Application.DTOs;
using SalaryAdvanced.Domain.Entities;

namespace SalaryAdvanced.Application.Interfaces
{
    public interface ISalaryAdvanceRequestService
    {
        Task<GetRequestDTO> CreateSalaryAdvanceRequestAsync(CreateRequestDTO dto);
        Task<IEnumerable<GetRequestDTO>> GetSalaryAdvanceRequestsByEmployeeAsync(int employeeId);
        Task<IEnumerable<GetRequestDTO>> GetSalaryAdvanceRequestsForApprovalAsync(int managerId);
        Task<IEnumerable<GetRequestDTO>> GetSalaryAdvanceRequestsByStatus(int statusId);
        Task<GetRequestDTO?> RespondToSalaryAdvanceRequestAsync(RequestResponseDTO dto);
        Task<bool> DeleteSalaryAdvanceRequestAsync(int id);
        Task<GetRequestDTO?> GetSalaryAdvanceRequestsById(int id);

    }
}