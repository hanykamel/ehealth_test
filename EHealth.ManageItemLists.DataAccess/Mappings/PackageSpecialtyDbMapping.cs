using EHealth.ManageItemLists.Domain.GlobelPackageTypes;
using EHealth.ManageItemLists.Domain.PackageSpecialties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.DataAccess.Mappings
{
    public class PackageSpecialtyDbMapping : IEntityTypeConfiguration<PackageSpecialty>
    {
        public void Configure(EntityTypeBuilder<PackageSpecialty> builder)
        {
            builder.ToTable("PackageSpecialties").HasKey(k => k.Id);
            builder.Property(k => k.SpecialtyAr).IsRequired().HasMaxLength(100);
            builder.Property(k => k.SpecialtyEn).IsRequired().HasMaxLength(100);
            builder.Property(k => k.DefinitionAr).HasMaxLength(1500);
            builder.Property(k => k.DefinitionEn).HasMaxLength(1500);
            builder.Property(k => k.Active).IsRequired().HasDefaultValue(true);
            builder.Property(k => k.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Ignore(x => x.Validator);
        }
    }
}
