using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostDepreciationsAndMaintenances;
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackageComponents;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.DataAccess.Mappings
{
    public class InvestmentCostPackageComponentDbMapping : IEntityTypeConfiguration<InvestmentCostPackageComponent>
    {
        public void Configure(EntityTypeBuilder<InvestmentCostPackageComponent> builder)
        {
            builder.ToTable("InvestmentCostPackageComponents").HasKey(k => k.Id);
            builder.HasOne(k => k.PackageHeader);
            builder.HasOne(k => k.FacilityUHIA);
            builder.HasOne(k => k.InvestmentCostDepreciationAndMaintenance).WithOne(k=> k.InvestmentCostPackageComponent).HasForeignKey<InvestmentCostDepreciationAndMaintenance>(k => k.InvestmentCostPackageComponentId);
            builder.HasMany(k => k.InvestmentCostPackagAssets);
            builder.Property(k => k.QuantityOfUnitsPerTheFacility).IsRequired();
            builder.Property(k => k.NumberOfSessionsPerUnitPerFacility).IsRequired();
            builder.Property(k => k.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Ignore(k => k.Validator);
        }
    }
}
