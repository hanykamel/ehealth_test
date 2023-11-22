using EHealth.ManageItemLists.Domain.DrugsPricing;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.DataAccess.Mappings
{
    public class DrugPriceDbMapping : IEntityTypeConfiguration<DrugPrice>
    {
        public void Configure(EntityTypeBuilder<DrugPrice> builder)
        {
            builder.ToTable("DrugsPrices").HasKey(k => k.Id);
            builder.Property(x=>x.MainUnitPrice).IsRequired();
            builder.Property(x=>x.FullPackPrice).IsRequired();
            builder.Property(x=>x.SubUnitPrice).IsRequired();
            builder.Property(k => k.EffectiveDateFrom).IsRequired();
            builder.Property(k => k.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Ignore(k => k.Validator);

        }
    }
}
