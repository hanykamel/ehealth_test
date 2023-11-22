using EHealth.ManageItemLists.Domain.UnitsTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EHealth.ManageItemLists.DataAccess.Mappings
{
    public class UnitsTypeDbMapping : IEntityTypeConfiguration<UnitsType>
    {
        public void Configure(EntityTypeBuilder<UnitsType> builder)
        {
            builder.ToTable("UnitsTypes").HasKey(k => k.Id);
            builder.Property(k => k.UnitAr).IsRequired();
            builder.Property(k => k.UnitEn).IsRequired();
            builder.Property(k => k.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Property(k => k.Active).IsRequired().HasDefaultValue(true);
            builder.Ignore(k => k.Validator);
        }
    }
}
