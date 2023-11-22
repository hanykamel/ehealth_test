using EHealth.ManageItemLists.Domain.Packages.FeesOfResourcesPerUnitPackage.FeesOfResourcesPerUnitPackageSummaries;
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostDepreciationsAndMaintenances;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.DataAccess.Mappings
{
    public class FeesOfResourcesPerUnitPackageSummaryDbMapping : IEntityTypeConfiguration<FeesOfResourcesPerUnitPackageSummary>
    {
        public void Configure(EntityTypeBuilder<FeesOfResourcesPerUnitPackageSummary> builder)
        {
            builder.ToTable("FeesOfResourcesPerUnitPackageSummaries").HasKey(k => k.Id);
            builder.HasOne(k => k.FeesOfResourcesPerUnitPackageComponent);
            builder.Property(k => k.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Ignore(k => k.Validator);
        }
    }
}
