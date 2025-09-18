namespace SalaryAdvanced.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IApplicationUserRepository ApplicationUsers { get; }
        IDepartmentRepository Departments { get; }
        IRequestStatusRepository RequestStatuses { get; }
        ISalaryAdvanceRequestRepository SalaryAdvanceRequests { get; }
        ISystemSettingRepository SystemSettings { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}