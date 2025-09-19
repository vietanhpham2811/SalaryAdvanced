using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalaryAdvanced.Domain.Entities;

public class LimitSalaryConfiguration : IEntityTypeConfiguration<LimitSalary>
{
    public void Configure(EntityTypeBuilder<LimitSalary> builder)
    {
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
            .HasColumnName("objectid") 
            .IsRequired();

        builder.Property(x => x.MaxOncePercent)
            .HasColumnName("maxoncepercent")  
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(x => x.MaxTimesPerMonth)
            .HasColumnName("maxtimespermonth") 
            .IsRequired();

        builder.Property(x => x.MaxMonthlyPercent)
            .HasColumnName("maxmonthlypercent") 
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(x => x.ValidFromDay)
            .HasColumnName("validfromday")  
            .IsRequired();

        builder.Property(x => x.ValidToDay)
            .HasColumnName("validtoday")  
            .IsRequired();
        builder.Property(x => x.create_date)
       .HasColumnName("create_date") // khớp cột "CreateDate"
       .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");

        builder.Property(x => x.last_change)
               .HasColumnName("last_change") // khớp cột "LastChange"
            .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");

    }
}
