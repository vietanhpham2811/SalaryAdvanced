using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalaryAdvanced.Domain.Entities;

namespace SalaryAdvanced.Infrastructure.Data.Configurations
{
    public class SalaryAdvanceRequestConfiguration : IEntityTypeConfiguration<SalaryAdvanceRequest>
    {
        public void Configure(EntityTypeBuilder<SalaryAdvanceRequest> builder)
        {
            builder.HasKey(sar => sar.Id);

            builder.Property(sar => sar.RequestCode)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(sar => sar.RequestCode)
                .IsUnique();

            builder.Property(sar => sar.Amount)
                .IsRequired()
                .HasColumnType("decimal(15,2)");

            builder.Property(sar => sar.Reason)
                .HasMaxLength(1000);

            builder.Property(sar => sar.SubmittedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(sar => sar.RejectionReason)
                .HasMaxLength(1000);

            // Relationship with Employee (requestor)
            builder.HasOne(sar => sar.Employee)
                .WithMany(e => e.SalaryAdvanceRequests)
                .HasForeignKey(sar => sar.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relationship with RequestStatus
            builder.HasOne(sar => sar.Status)
                .WithMany(rs => rs.SalaryAdvanceRequests)
                .HasForeignKey(sar => sar.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relationship with Employee (approver)
            builder.HasOne(sar => sar.ApprovedBy)
                .WithMany(e => e.ApprovedRequests)
                .HasForeignKey(sar => sar.ApprovedById)
                .OnDelete(DeleteBehavior.SetNull);

            // Indexes for performance
            builder.HasIndex(sar => sar.EmployeeId);
            builder.HasIndex(sar => sar.StatusId);
            builder.HasIndex(sar => sar.SubmittedAt);
            builder.HasIndex(sar => sar.ProcessedAt);
        }
    }
}