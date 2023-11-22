using EHealth.ManageItemLists.Domain.ItemListSubtypes;
using EHealth.ManageItemLists.Domain.PackageSubTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.DataAccess.Mappings
{
    public class PackageSubTypeDbMapping : IEntityTypeConfiguration<PackageSubType>
    {
        public void Configure(EntityTypeBuilder<PackageSubType> builder)
        {
            builder.ToTable("PackageSubTypes").HasKey(k => k.Id);
            builder.HasOne(k => k.PackageType);
            builder.Property(k => k.NameAr).IsRequired().HasMaxLength(100);
            builder.Property(k => k.NameEN).IsRequired().HasMaxLength(100);
            builder.Property(k => k.Active).IsRequired().HasDefaultValue(true);
            builder.Property(k => k.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Ignore(x => x.Validator);
        }
    }
}
