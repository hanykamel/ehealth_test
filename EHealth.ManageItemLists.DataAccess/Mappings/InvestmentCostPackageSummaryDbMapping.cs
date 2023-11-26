using EHealth.ManageItemLists.Domain.Packages.FeesOfResourcesPerUnitPackage.FeesOfResourcesPerUnitPackageSummaries;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackageSummaries;

namespace EHealth.ManageItemLists.DataAccess.Mappings
{
    public class InvestmentCostPackageSummaryDbMapping : IEntityTypeConfiguration<InvestmentCostPackageSummary>
    {
        public void Configure(EntityTypeBuilder<InvestmentCostPackageSummary> builder)
        {
            builder.ToTable("InvestmentCostPackageSummaries").HasKey(k => k.Id);
            builder.HasOne(k => k.InvestmentCostPackageComponent);
            builder.Property(k => k.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Ignore(k => k.Validator);
        }
    }
}
