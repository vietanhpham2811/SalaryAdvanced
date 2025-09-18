using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalaryAdvanced.Domain.Entities;

namespace SalaryAdvanced.Infrastructure.Data.Configurations
{
    public class RequestStatusConfiguration : IEntityTypeConfiguration<RequestStatus>
    {
        public void Configure(EntityTypeBuilder<RequestStatus> builder)
        {
            builder.HasKey(rs => rs.Id);

            builder.Property(rs => rs.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(rs => rs.Name)
                .IsUnique();

            builder.Property(rs => rs.Description)
                .HasMaxLength(500);

            // Seed data
            builder.HasData(
                new RequestStatus { Id = 1, Name = "Pending", Description = "Chờ phê duyệt", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new RequestStatus { Id = 2, Name = "Approved", Description = "Đã phê duyệt", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new RequestStatus { Id = 3, Name = "Rejected", Description = "Đã từ chối", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            );
        }
    }
}