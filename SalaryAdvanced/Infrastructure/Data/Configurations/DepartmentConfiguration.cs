using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalaryAdvanced.Domain.Entities;

namespace SalaryAdvanced.Infrastructure.Data.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(e => e.Code)
                .IsUnique();

            builder.Property(e => e.Description)
                .HasMaxLength(1000);

            // Self-reference relationship for Manager (ApplicationUser)
            builder.HasOne(d => d.Manager)
                .WithOne(u => u.ManagedDepartment)
                .HasForeignKey<Department>(d => d.ManagerId)
                .OnDelete(DeleteBehavior.SetNull);

            // One-to-many relationship with ApplicationUsers
            builder.HasMany(d => d.Employees)
                .WithOne(u => u.Department)
                .HasForeignKey(u => u.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}