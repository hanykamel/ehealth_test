using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackagAssets.DTOs
{
    public class UpdateInvestmentCostAssetsDto
    {
        public Guid Id { get; set; }
        public Guid InvestmentCostPackageComponentId { get; set; }
        public Guid DevicesAndAssetsUHIAId { get; set; }
        public int Quantity { get; set; }
        public double? YearlyDepreciationPercentage { get; set; }
        public double YearlyMaintenancePercentage { get; set; }
        public double? TotalCost { get; set; }
        public double? YearlyDepreciationCostForTheAddedAssets { get; set; }
        public double? YearlyMaintenanceCostForTheAddedAsset { get; set; }
    }
}
