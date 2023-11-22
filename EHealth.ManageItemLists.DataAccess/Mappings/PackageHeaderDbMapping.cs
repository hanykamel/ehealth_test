using EHealth.ManageItemLists.Domain.Packages.PackageHeaders;
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
    public class PackageHeaderDbMapping : IEntityTypeConfiguration<PackageHeader>
    {
        public void Configure(EntityTypeBuilder<PackageHeader> builder)
        {
            builder.ToTable("PackageHeaders").HasKey(k => k.Id);
            builder.Property(k => k.UHIACode).IsRequired();
            builder.Property(k => k.NameEn).IsRequired().HasMaxLength(1000);
            builder.Property(k => k.NameAr).HasMaxLength(1000);
            builder.HasOne(k => k.PackageType);
            builder.HasOne(k => k.PackageSubType);
            builder.HasOne(k => k.GlobelPackageType);
            builder.HasOne(k => k.PackageSpecialty);
            builder.Property(k => k.PackageDuration).IsRequired();
            builder.Property(k => k.ActivationDateFrom).IsRequired();
            builder.Property(k => k.PackagePrice).IsRequired();
            builder.Property(k => k.PackageRoundPrice).IsRequired();
            builder.Property(k => k.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Ignore(x => x.Validator);
        }
    }
}
