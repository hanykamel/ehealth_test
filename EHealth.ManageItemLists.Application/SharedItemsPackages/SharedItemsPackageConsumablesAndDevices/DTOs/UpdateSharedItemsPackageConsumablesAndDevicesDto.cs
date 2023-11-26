using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.SharedItemsPackages.SharedItemsPackageConsumablesAndDevices.DTOs
{
    public class UpdateSharedItemsPackageConsumablesAndDevicesDto
    {
        public Guid Id { get; set; }
        public Guid SharedItemsPackageComponentId { get; set; }
        public Guid ConsumablesAndDevicesUHIAId { get; set; }
        public int Quantity { get; set; }
        public int NumberOfCasesInTheUnit { get; set; }
        public int? LocationId { get; set; }
        public double? TotalCost { get; set; }
        public double? ConsumablePerCase { get; set; }
    }
}
