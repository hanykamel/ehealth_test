using EHealth.ManageItemLists.Domain.Packages.FeesOfResourcesPerUnitPackage.FeesOfResourcesPerUnitPackageResources;
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackagAssets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.DataAccess.Mappings
{
    public class FeesOfResourcesPerUnitPackageResourceDbMapping : IEntityTypeConfiguration<FeesOfResourcesPerUnitPackageResource>
    {
        public void Configure(EntityTypeBuilder<FeesOfResourcesPerUnitPackageResource> builder)
        {
            builder.ToTable("FeesOfResourcesPerUnitPackageResources").HasKey(k => k.Id);
            builder.HasOne(k => k.FeesOfResourcesPerUnitPackageComponent);
            builder.HasOne(k => k.ResourceUHIA);
            builder.Property(k => k.Quantity).IsRequired();
            builder.Property(k => k.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Ignore(k => k.Validator);
        }
    }
}
