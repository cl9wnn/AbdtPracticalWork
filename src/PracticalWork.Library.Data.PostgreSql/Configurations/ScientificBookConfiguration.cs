using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PracticalWork.Library.Data.PostgreSql.Entities;

namespace PracticalWork.Library.Data.PostgreSql.Configurations;

internal sealed class ScientificBookConfiguration : EntityConfigurationBase<ScientificBookEntity>
{
    public override void Configure(EntityTypeBuilder<ScientificBookEntity> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.ResearchField)
            .HasMaxLength(100);
        
        builder.Property(e => e.Publisher)
            .HasMaxLength(100);
    }
}