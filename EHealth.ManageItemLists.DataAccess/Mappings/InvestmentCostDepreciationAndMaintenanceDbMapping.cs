using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostDepreciationsAndMaintenances;
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
    public class InvestmentCostDepreciationAndMaintenanceDbMapping : IEntityTypeConfiguration<InvestmentCostDepreciationAndMaintenance>
    {
        public void Configure(EntityTypeBuilder<InvestmentCostDepreciationAndMaintenance> builder)
        {
            builder.ToTable("InvestmentCostDepreciationsAndMaintenances").HasKey(k => k.Id);
            builder.HasOne(k => k.InvestmentCostPackageComponent);
            builder.Property(k => k.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Ignore(k => k.Validator);
        }
    }
}
