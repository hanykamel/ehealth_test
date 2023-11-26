using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackageSummaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackagAssets.DTOs
{
    public class InvestmentCostSummaryDto
    {
        public int? NumberOfItems { get; set; }
        public int? TotalNumberOfQuantity { get; set; }
        public double? TotalCostOfItems { get; set; }
        public double? YearlyDepreciationCostForTheAddedAsset { get; set; }
        public double? YearlyMaintenanceCostForTheAddedAsset { get; set; }

        public static InvestmentCostSummaryDto? FromInvestmentCostSummaryDto(InvestmentCostPackageSummary input) =>
        input is not null ? new InvestmentCostSummaryDto
        {
            NumberOfItems = input.NumberOfItems,
            TotalNumberOfQuantity = input.TotalNumberOfQuantity,
            TotalCostOfItems = input.TotalCostOfItems,
            YearlyDepreciationCostForTheAddedAsset = input.YearlyDepreciationCostForTheAddedAsset,
            YearlyMaintenanceCostForTheAddedAsset = input.YearlyMaintenanceCostForTheAddedAsset
        } : null;
    }
}
