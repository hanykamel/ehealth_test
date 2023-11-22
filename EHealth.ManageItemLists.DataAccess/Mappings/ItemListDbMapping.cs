using EHealth.ManageItemLists.Domain.ItemLists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EHealth.ManageItemLists.DataAccess.Mappings
{
    public class ItemListDbMapping : IEntityTypeConfiguration<ItemList>
    {
        public void Configure(EntityTypeBuilder<ItemList> builder)
        {
            builder.ToTable("ItemLists").HasKey(k => k.Id);
            builder.Property(k => k.Code).IsRequired();
            builder.HasOne(k => k.ItemListSubtype);
            builder.Property(k => k.NameAr).IsRequired().HasMaxLength(100);
            builder.Property(k => k.NameEN).IsRequired().HasMaxLength(100);
            builder.Property(k => k.Active).IsRequired().HasDefaultValue(true);
            builder.Property(k => k.IsBusy).IsRequired().HasDefaultValue(false);
            builder.Ignore(x => x.Validator);
        }
    }
}
