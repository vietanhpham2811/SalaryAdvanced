using Microsoft.EntityFrameworkCore.Storage;
using SalaryAdvanced.Domain.Interfaces;
using SalaryAdvanced.Infrastructure.Data;

namespace SalaryAdvanced.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            
            ApplicationUsers = new ApplicationUserRepository(_context);
            Departments = new DepartmentRepository(_context);
            RequestStatuses = new RequestStatusRepository(_context);
            SalaryAdvanceRequests = new SalaryAdvanceRequestRepository(_context);
            SystemSettings = new SystemSettingRepository(_context);
        }

        public IApplicationUserRepository ApplicationUsers { get; }
        public IDepartmentRepository Departments { get; }
        public IRequestStatusRepository RequestStatuses { get; }
        public ISalaryAdvanceRequestRepository SalaryAdvanceRequests { get; }
        public ISystemSettingRepository SystemSettings { get; }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}