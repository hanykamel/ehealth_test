using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.PackageComplexityClassifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.DataAccess.Mappings
{
    public class PackageComplexityClassificationDbMapping : IEntityTypeConfiguration<PackageComplexityClassification>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<PackageComplexityClassification> builder)
        {
            builder.ToTable("PackageComplexityClassifications").HasKey(k => k.Id);
            builder.Property(k => k.Code).IsRequired();
            builder.Property(k => k.ComplexityAr).IsRequired().HasMaxLength(100);
            builder.Property(k => k.ComplexityEn).IsRequired().HasMaxLength(100);
            builder.Property(k => k.DefinitionAr).HasMaxLength(1500);
            builder.Property(k => k.DefinitionEn).HasMaxLength(1500);
            builder.Property(k => k.Active).IsRequired().HasDefaultValue(true);
            builder.Property(k => k.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Ignore(x => x.Validator);
        }
    }
}
