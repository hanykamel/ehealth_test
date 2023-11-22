using EHealth.ManageItemLists.Domain.ItemListPricing;
using EHealth.ManageItemLists.Domain.Resource.ItemPrice;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.DataAccess.Mappings
{
    public class ResourceItemPriceDbMapping : IEntityTypeConfiguration<ResourceItemPrice>
    {
        public void Configure(EntityTypeBuilder<ResourceItemPrice> builder)
        {
            builder.ToTable("ResourceItemPrices").HasKey(k => k.Id);
            builder.Property(k => k.Price).IsRequired().HasPrecision(11,7);
            builder.HasOne(k => k.PriceUnit);
            builder.Property(k => k.EffectiveDateFrom).IsRequired();
            builder.Property(k => k.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Ignore(x => x.Validator);
        }
    }
}
