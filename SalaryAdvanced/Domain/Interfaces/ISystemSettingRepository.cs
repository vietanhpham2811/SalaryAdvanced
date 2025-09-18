using SalaryAdvanced.Domain.Entities;

namespace SalaryAdvanced.Domain.Interfaces
{
    public interface ISystemSettingRepository : IRepository<SystemSetting>
    {
        Task<SystemSetting?> GetByKeyAsync(string key);
        Task<string?> GetValueAsync(string key);
        Task<T?> GetValueAsync<T>(string key) where T : struct;
        Task UpdateValueAsync(string key, string value);
    }
}