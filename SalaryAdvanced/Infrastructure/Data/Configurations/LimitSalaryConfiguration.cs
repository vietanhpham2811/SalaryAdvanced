using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalaryAdvanced.Domain.Entities;

public class LimitSalaryConfiguration : IEntityTypeConfiguration<LimitSalary>
{
    public void Configure(EntityTypeBuilder<LimitSalary> builder)
    {
        // Đặt tên bảng là "limitsalary" (không dấu gạch dưới)
        builder.ToTable("limitsalary");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Type)
            .HasColumnName("type")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.ObjectId)
            .HasColumnName("objectid")  // không gạch dưới theo yêu cầu
            .IsRequired();

        builder.Property(x => x.MaxOncePercent)
            .HasColumnName("maxoncepercent")  // không gạch dưới
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(x => x.MaxTimesPerMonth)
            .HasColumnName("maxtimespermonth") // không gạch dưới
            .IsRequired();

        builder.Property(x => x.MaxMonthlyPercent)
            .HasColumnName("maxmonthlypercent")  // không gạch dưới
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(x => x.ValidFromDay)
            .HasColumnName("validfromday")  // không gạch dưới
            .IsRequired();

        builder.Property(x => x.ValidToDay)
            .HasColumnName("validtoday")  // không gạch dưới
            .IsRequired();
    }
}
