using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PracticalWork.Library.Data.PostgreSql.Entities;

namespace PracticalWork.Library.Data.PostgreSql.Configurations;

internal sealed class EducationalBookConfiguration : EntityConfigurationBase<EducationalBookEntity>
{
    public override void Configure(EntityTypeBuilder<EducationalBookEntity> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.Subject)
            .HasMaxLength(100);
        
        builder.Property(e => e.GradeLevel)
            .HasMaxLength(100);
    }
}