using EHealth.ManageItemLists.Domain.ConsumablesAndDevices;
using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackagAssets;
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackageComponents;
using EHealth.ManageItemLists.Domain.Packages.SharedItemsPackages.SharedItemsPackageComponents;
using EHealth.ManageItemLists.Domain.Packages.SharedItemsPackages.SharedItemsPackageConsumablesAndDevices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.SharedItemsPackages.SharedItemsPackageConsumablesAndDevices.DTOs
{
    public class CreateSharedItemsPackageConsumablesAndDevicesDto
    {
        public Guid SharedItemsPackageComponentId { get; set; }
        public Guid ConsumablesAndDevicesUHIAId { get; set; }
        public int Quantity { get; set; }
        public int NumberOfCasesInTheUnit { get; set; }
        public int? LocationId { get; set; }
        public double? TotalCost { get; set; }
        public double? ConsumablePerCase { get; set; }

        public SharedItemsPackageConsumableAndDevice ToSharedItemsPackageConsumableAndDevice(string createdBy, string tenantId) => 
            SharedItemsPackageConsumableAndDevice.Create(null, SharedItemsPackageComponentId, ConsumablesAndDevicesUHIAId, Quantity,
            NumberOfCasesInTheUnit, LocationId, TotalCost, ConsumablePerCase, createdBy, tenantId);
    }
}
