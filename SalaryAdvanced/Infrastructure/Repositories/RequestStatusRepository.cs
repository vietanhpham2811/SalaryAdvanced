using Microsoft.EntityFrameworkCore;
using SalaryAdvanced.Domain.Entities;
using SalaryAdvanced.Domain.Interfaces;
using SalaryAdvanced.Infrastructure.Data;

namespace SalaryAdvanced.Infrastructure.Repositories
{
    public class RequestStatusRepository : Repository<RequestStatus>, IRequestStatusRepository
    {
        public RequestStatusRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<RequestStatus?> GetByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(rs => rs.Name == name);
        }
    }
}