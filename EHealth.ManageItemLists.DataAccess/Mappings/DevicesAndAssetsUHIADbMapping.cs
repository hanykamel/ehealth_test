using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.DataAccess.Mappings
{
    public class DevicesAndAssetsUHIADbMapping : IEntityTypeConfiguration<DevicesAndAssetsUHIA>
    {
        public void Configure(EntityTypeBuilder<DevicesAndAssetsUHIA> builder)
        {
            builder.ToTable("DevicesAndAssetsUHIA").HasKey(k => k.Id);
            builder.Property(k => k.Code).IsRequired();
            builder.Property(k => k.DescriptorAr).IsRequired().HasMaxLength(100);
            builder.Property(k => k.DescriptorEn).IsRequired().HasMaxLength(100);
            builder.HasOne(k => k.ItemList);
            builder.HasOne(k => k.SubCategory);
            builder.HasOne(k => k.Category);
            builder.HasOne(k => k.UnitRoom);
            builder.HasMany(k => k.ItemListPrices);
            builder.Property(k => k.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Ignore(x => x.Validator);
        }
    }
}
