using EHealth.ManageItemLists.Domain.ItemListSubtypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EHealth.ManageItemLists.DataAccess.Mappings
{
    public class ItemListSubtypeDbMapping : IEntityTypeConfiguration<ItemListSubtype>
    {
        public void Configure(EntityTypeBuilder<ItemListSubtype> builder)
        {
            builder.ToTable("ItemListSubtypes").HasKey(k => k.Id);
            builder.Property(k => k.Code).IsRequired();
            builder.HasOne(k => k.ItemListType);
            builder.HasMany(k => k.ItemLists);
            builder.Property(k => k.NameAr).IsRequired().HasMaxLength(100);
            builder.Property(k => k.NameEN).IsRequired().HasMaxLength(100);
            builder.Property(k => k.Active).IsRequired().HasDefaultValue(true);
            builder.Ignore(x => x.Validator);
        }
    }
}
