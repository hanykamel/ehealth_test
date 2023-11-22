using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.ItemListTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.DataAccess.Mappings
{
    public class ItemListTypeDbMapping : IEntityTypeConfiguration<ItemListType>
    {
        public void Configure(EntityTypeBuilder<ItemListType> builder)
        {
            builder.ToTable("ItemListTypes").HasKey(k => k.Id);
            builder.Property(k => k.Code).IsRequired();
            builder.HasMany(k => k.ItemListSubtypes);
            builder.Property(k => k.NameAr).IsRequired().HasMaxLength(100);
            builder.Property(k => k.NameEN).IsRequired().HasMaxLength(100);
            builder.Property(k => k.Active).IsRequired().HasDefaultValue(true);
            builder.Ignore(x => x.Validator);
        }
    }
}
