using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EHealth.ManageItemLists.DataAccess.Mappings
{
    public class ServicesUHIADbMapping : IEntityTypeConfiguration<ServiceUHIA>
    {
        public void Configure(EntityTypeBuilder<ServiceUHIA> builder)
        {
            builder.ToTable("ServicesUHIA").HasKey(k => k.Id);
            builder.Property(k => k.EHealthCode).IsRequired();
            builder.Property(k => k.UHIAId).IsRequired();
            builder.HasOne(k => k.ServiceCategory);
            builder.HasOne(k => k.ServiceSubCategory);
            builder.HasOne(k => k.ItemList);
            builder.HasMany(k => k.ItemListPrices);
            builder.Property(k => k.DataEffectiveDateFrom).IsRequired();
            builder.Property(k => k.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Ignore(k => k.Validator);
        }
    }
}
