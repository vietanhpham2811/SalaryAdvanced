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
                new SystemSetting { Id = 1, SettingKey = "MAX_ADVANCE_PERCENTAGE", SettingValue = "50", Description = "Maximum percentage per salary advance", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new SystemSetting { Id = 2, SettingKey = "MAX_MONTHLY_PERCENTAGE", SettingValue = "70", Description = "Maximum monthly percentage", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new SystemSetting { Id = 3, SettingKey = "MAX_REQUESTS_PER_MONTH", SettingValue = "2", Description = "Maximum requests per month", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new SystemSetting { Id = 4, SettingKey = "REQUEST_START_DAY", SettingValue = "1", Description = "Start day for requests", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new SystemSetting { Id = 5, SettingKey = "REQUEST_END_DAY", SettingValue = "25", Description = "End day for requests", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            );
        }
    }
}