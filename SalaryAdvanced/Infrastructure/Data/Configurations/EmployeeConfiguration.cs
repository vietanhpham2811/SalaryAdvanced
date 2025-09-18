using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalaryAdvanced.Domain.Entities;

namespace SalaryAdvanced.Infrastructure.Data.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.EmployeeCode)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(e => e.EmployeeCode)
                .IsUnique();

            builder.Property(e => e.FullName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasIndex(e => e.Email)
                .IsUnique();

            builder.Property(e => e.Phone)
                .HasMaxLength(20);

            builder.Property(e => e.PasswordHash)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(e => e.BasicSalary)
                .IsRequired()
                .HasColumnType("decimal(15,2)");

            builder.Property(e => e.HireDate)
                .IsRequired();

            builder.Property(e => e.IsActive)
                .HasDefaultValue(true);

            // Relationship with Department
            builder.HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relationship with Role
            builder.HasOne(e => e.Role)
                .WithMany(r => r.Employees)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relationship with SalaryAdvanceRequests (as requestor)
            builder.HasMany(e => e.SalaryAdvanceRequests)
                .WithOne(r => r.Employee)
                .HasForeignKey(r => r.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relationship with SalaryAdvanceRequests (as approver)
            builder.HasMany(e => e.ApprovedRequests)
                .WithOne(r => r.ApprovedBy)
                .HasForeignKey(r => r.ApprovedById)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}