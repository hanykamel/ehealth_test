using EHealth.ManageItemLists.Domain.ItemListPricing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EHealth.ManageItemLists.DataAccess.Mappings
{
    public class ItemListPriceDbMapping : IEntityTypeConfiguration<ItemListPrice>
    {
        public void Configure(EntityTypeBuilder<ItemListPrice> builder)
        {
            builder.ToTable("ItemListPrices").HasKey(k => k.Id);
            builder.Property(k => k.Price).IsRequired();
            builder.Property(k => k.EffectiveDateFrom).IsRequired();
            builder.Property(k => k.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Ignore(x => x.Validator);
        }
    }
}
