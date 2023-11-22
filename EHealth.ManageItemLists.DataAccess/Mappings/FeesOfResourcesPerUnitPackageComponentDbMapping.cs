using EHealth.ManageItemLists.Domain.Packages.FeesOfResourcesPerUnitPackage.FeesOfResourcesPerUnitPackageComponents;
using EHealth.ManageItemLists.Domain.Packages.FeesOfResourcesPerUnitPackage.FeesOfResourcesPerUnitPackageSummaries;
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostDepreciationsAndMaintenances;
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackageComponents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.DataAccess.Mappings
{
    public class FeesOfResourcesPerUnitPackageComponentDbMapping : IEntityTypeConfiguration<FeesOfResourcesPerUnitPackageComponent>
    {
        public void Configure(EntityTypeBuilder<FeesOfResourcesPerUnitPackageComponent> builder)
        {
            builder.ToTable("FeesOfResourcesPerUnitPackageComponents").HasKey(k => k.Id);
            builder.HasOne(k => k.PackageHeader);
            builder.HasOne(k => k.FacilityUHIA);
            builder.HasOne(k => k.FeesOfResourcesPerUnitPackageSummary).WithOne(k => k.FeesOfResourcesPerUnitPackageComponent).HasForeignKey<FeesOfResourcesPerUnitPackageSummary>(k => k.FeesOfResourcesPerUnitPackageComponentId);
            builder.HasMany(k => k.FeesOfResourcesPerUnitPackageResources);
            builder.Property(k => k.QuantityOfUnitsPerTheFacility).IsRequired();
            builder.Property(k => k.NumberOfSessionsPerUnitPerFacility).IsRequired();
            builder.Property(k => k.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Ignore(k => k.Validator);
        }
    }
}
