using EHealth.ManageItemLists.Domain.ConsumablesAndDevices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EHealth.ManageItemLists.DataAccess.Mappings
{
    public class ConsumablesAndDevicesUHIADbMapping : IEntityTypeConfiguration<ConsumablesAndDevicesUHIA>
    {
        public void Configure(EntityTypeBuilder<ConsumablesAndDevicesUHIA> builder)
        {
            builder.ToTable("ConsumablesAndDevicesUHIA").HasKey(k => k.Id);
            builder.Property(k => k.EHealthCode).IsRequired();
            builder.Property(k => k.ShortDescriptorEn).IsRequired();
            builder.HasOne(k => k.UnitOfMeasure);
            builder.HasOne(k => k.ServiceCategory);
            builder.HasOne(k => k.SubCategory);
            builder.HasOne(k => k.ItemList);
            builder.HasMany(k => k.ItemListPrices);
            builder.Property(k => k.DataEffectiveDateFrom).IsRequired();
            builder.Property(k => k.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Ignore(k => k.Validator);

        }
    }
}
