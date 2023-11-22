using EHealth.ManageItemLists.Domain.GlobelPackageTypes;
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
    public class GlobelPackageTypeDbMapping : IEntityTypeConfiguration<GlobelPackageType>
    {
        public void Configure(EntityTypeBuilder<GlobelPackageType> builder)
        {
            builder.ToTable("GlobelPackageTypes").HasKey(k => k.Id);
            builder.Property(k => k.GlobalTypeAr).IsRequired().HasMaxLength(100);
            builder.Property(k => k.GlobalTypeEn).IsRequired().HasMaxLength(100);
            builder.Property(k => k.DefinitionAr).HasMaxLength(1500);
            builder.Property(k => k.DefinitionEn).HasMaxLength(1500);
            builder.Property(k => k.Active).IsRequired().HasDefaultValue(true);
            builder.Property(k => k.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Ignore(x => x.Validator);
        }
    }
}
