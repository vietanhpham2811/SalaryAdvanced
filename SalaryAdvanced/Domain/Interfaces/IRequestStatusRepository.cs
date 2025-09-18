using SalaryAdvanced.Domain.Entities;

namespace SalaryAdvanced.Domain.Interfaces
{
    public interface IRequestStatusRepository : IRepository<RequestStatus>
    {
        Task<RequestStatus?> GetByNameAsync(string name);
    }
}