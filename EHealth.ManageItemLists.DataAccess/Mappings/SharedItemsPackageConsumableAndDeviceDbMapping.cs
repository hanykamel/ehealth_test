using EHealth.ManageItemLists.Domain.Packages.SharedItemsPackages.SharedItemsPackageConsumablesAndDevices;
using EHealth.ManageItemLists.Domain.Packages.SharedItemsPackages.SharedItemsPackageDrugs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.DataAccess.Mappings
{
    public class SharedItemsPackageConsumableAndDeviceDbMapping : IEntityTypeConfiguration<SharedItemsPackageConsumableAndDevice>
    {
        public void Configure(EntityTypeBuilder<SharedItemsPackageConsumableAndDevice> builder)
        {
            builder.ToTable("SharedItemsPackageConsumablesAndDevices").HasKey(k => k.Id);
            builder.HasOne(k => k.SharedItemsPackageComponent);
            builder.HasOne(k => k.ConsumablesAndDevicesUHIA);
            builder.Property(k => k.Quantity).IsRequired();
            builder.Property(k => k.NumberOfCasesInTheUnit).IsRequired();
            builder.Property(k => k.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Ignore(k => k.Validator);
        }
    }
}
