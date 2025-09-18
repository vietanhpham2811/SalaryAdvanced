using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalaryAdvanced.Domain.Entities;

namespace SalaryAdvanced.Infrastructure.Data.Configurations
{
    public class SystemSettingConfiguration : IEntityTypeConfiguration<SystemSetting>
    {
        public void Configure(EntityTypeBuilder<SystemSetting> builder)
        {
            builder.HasKey(ss => ss.Id);

            builder.Property(ss => ss.SettingKey)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(ss => ss.SettingKey)
                .IsUnique();

            builder.Property(ss => ss.SettingValue)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(ss => ss.Description)
                .HasMaxLength(1000);

            // Seed data
            builder.HasData(
                new SystemSetting { Id = 1, SettingKey = "MAX_ADVANCE_PERCENTAGE", SettingValue = "50", Description = "Tỷ lệ % tối đa mỗi lần ứng lương", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new SystemSetting { Id = 2, SettingKey = "MAX_MONTHLY_PERCENTAGE", SettingValue = "70", Description = "Tỷ lệ % tối đa hàng tháng", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new SystemSetting { Id = 3, SettingKey = "MAX_REQUESTS_PER_MONTH", SettingValue = "2", Description = "Số lần ứng tối đa mỗi tháng", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new SystemSetting { Id = 4, SettingKey = "REQUEST_START_DAY", SettingValue = "1", Description = "Ngày bắt đầu có thể gửi yêu cầu", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new SystemSetting { Id = 5, SettingKey = "REQUEST_END_DAY", SettingValue = "25", Description = "Ngày kết thúc có thể gửi yêu cầu", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            );
        }
    }
}