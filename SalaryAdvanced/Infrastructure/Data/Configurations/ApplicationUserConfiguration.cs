using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalaryAdvanced.Domain.Entities;

namespace SalaryAdvanced.Infrastructure.Data.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.EmployeeCode)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(u => u.EmployeeCode)
                .IsUnique();

            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.BasicSalary)
                .IsRequired()
                .HasColumnType("decimal(15,2)");

            builder.Property(u => u.HireDate)
                .IsRequired();

            builder.Property(u => u.IsActive)
                .HasDefaultValue(true);

            // Relationship with Department
            builder.HasOne(u => u.Department)
                .WithMany()
                .HasForeignKey(u => u.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Note: Role relationship is handled by Identity framework through ApplicationRole
            
            // Relationship with SalaryAdvanceRequests (as requestor)
            builder.HasMany(u => u.SalaryAdvanceRequests)
                .WithOne(r => r.ApplicationUser)
                .HasForeignKey(r => r.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relationship with SalaryAdvanceRequests (as approver)
            builder.HasMany(u => u.ApprovedRequests)
                .WithOne(r => r.ApprovedByUser)
                .HasForeignKey(r => r.ApprovedById)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure table name
            builder.ToTable("application_users");
        }
    }
}