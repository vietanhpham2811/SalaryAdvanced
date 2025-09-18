using Microsoft.EntityFrameworkCore;
using SalaryAdvanced.Domain.Entities;
using SalaryAdvanced.Domain.Interfaces;
using SalaryAdvanced.Infrastructure.Data;

namespace SalaryAdvanced.Infrastructure.Repositories
{
    public class SystemSettingRepository : Repository<SystemSetting>, ISystemSettingRepository
    {
        public SystemSettingRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<SystemSetting?> GetByKeyAsync(string key)
        {
            return await _dbSet.FirstOrDefaultAsync(s => s.SettingKey == key);
        }

        public async Task<string?> GetValueAsync(string key)
        {
            var setting = await GetByKeyAsync(key);
            return setting?.SettingValue;
        }

        public async Task<T?> GetValueAsync<T>(string key) where T : struct
        {
            var value = await GetValueAsync(key);
            if (string.IsNullOrEmpty(value))
                return null;

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return null;
            }
        }

        public async Task UpdateValueAsync(string key, string value)
        {
            var setting = await GetByKeyAsync(key);
            if (setting != null)
            {
                setting.SettingValue = value;
                setting.UpdatedAt = DateTime.UtcNow;
                Update(setting);
            }
        }
    }
}