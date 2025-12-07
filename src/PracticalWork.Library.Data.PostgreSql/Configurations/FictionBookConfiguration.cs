using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PracticalWork.Library.Data.PostgreSql.Entities;

namespace PracticalWork.Library.Data.PostgreSql.Configurations;

internal sealed class FictionBookConfiguration : EntityConfigurationBase<FictionBookEntity>
{
    public override void Configure(EntityTypeBuilder<FictionBookEntity> builder)
    {
        base.Configure(builder);

        builder.Property(b => b.CategoriesOfFiction)
            .HasMaxLength(100);
    }
}