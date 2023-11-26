using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostDepreciationsAndMaintenances;
using EHealth.ManageItemLists.Domain.Packages.SharedItemsPackages.SharedItemsPackageComponents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.DataAccess.Mappings
{
    public class SharedItemsPackageComponentDbMapping : IEntityTypeConfiguration<SharedItemsPackageComponent>
    {
        public void Configure(EntityTypeBuilder<SharedItemsPackageComponent> builder)
        {
            builder.ToTable("SharedItemsPackageComponents").HasKey(k => k.Id);
            builder.HasOne(k => k.PackageHeader);
            builder.HasMany(k => k.SharedItemsPackageDrugs);
            builder.HasMany(k => k.SharedItemsPackageConsumablesAndDevices);
            builder.Property(k => k.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Ignore(k => k.Validator);
        }
    }
}
