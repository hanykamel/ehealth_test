using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackagAssets;
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
    public class InvestmentCostPackageAssetDbMapping : IEntityTypeConfiguration<InvestmentCostPackageAsset>
    {
        public void Configure(EntityTypeBuilder<InvestmentCostPackageAsset> builder)
        {
            builder.ToTable("InvestmentCostPackageAssets").HasKey(k => k.Id);
            builder.HasOne(k => k.InvestmentCostPackageComponent);
            builder.HasOne(k => k.DevicesAndAssetsUHIA);
            builder.Property(k => k.Quantity).IsRequired();
            builder.Property(k => k.YearlyMaintenancePercentage).IsRequired();
            builder.Property(k => k.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Ignore(k => k.Validator);
        }
    }
}
