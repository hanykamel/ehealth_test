using EHealth.ManageItemLists.Domain.Facility.UHIA;
using EHealth.ManageItemLists.Domain.Resource.UHIA;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.DataAccess.Mappings
{
    public class ResourceUHIADbMapping : IEntityTypeConfiguration<ResourceUHIA>
    {
        public void Configure(EntityTypeBuilder<ResourceUHIA> builder)
        {
            builder.ToTable("ResourceUHIA").HasKey(k => k.Id);
            builder.Property(k => k.Code).IsRequired();
            builder.Property(k => k.DescriptorAr).HasMaxLength(100);
            builder.Property(k => k.DescriptorEn).IsRequired().HasMaxLength(100);
            builder.HasOne(k => k.ItemList);
            builder.HasOne(k => k.SubCategory);
            builder.HasOne(k => k.Category);
            builder.HasMany(k => k.ItemListPrices);
            builder.Property(k => k.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Ignore(x => x.Validator);
        }
    }
}
