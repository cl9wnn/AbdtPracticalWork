using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PracticalWork.Reports.Data.PostgreSql.Entities;

namespace PracticalWork.Reports.Data.PostgreSql.Configurations;

public class ActivityLogConfiguration : EntityConfigurationBase<ActivityLogEntity>
{
    public override void Configure(EntityTypeBuilder<ActivityLogEntity> builder)
    {
        base.Configure(builder);
        
        builder.Property(x => x.EventType)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.EventDate)
            .IsRequired();

        builder.Property(x => x.Metadata)
            .HasColumnType("jsonb");

        builder.Property(x => x.BookId)
            .HasColumnType("uuid");

        builder.Property(x => x.ReaderId)
            .HasColumnType("uuid");
        
        builder.HasIndex(x => x.EventDate);
        builder.HasIndex(x => x.EventType);
        builder.HasIndex(x => x.BookId);
        builder.HasIndex(x => x.ReaderId);
    }
}