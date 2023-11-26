using DocumentFormat.OpenXml.Wordprocessing;
using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using EHealth.ManageItemLists.Domain.GlobelPackageTypes;
using EHealth.ManageItemLists.Domain.PackageComplexityClassifications;
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackagAssets;
using EHealth.ManageItemLists.Domain.Packages.PackageHeaders;
using EHealth.ManageItemLists.Domain.PackageSpecialties;
using EHealth.ManageItemLists.Domain.PackageSubTypes;
using EHealth.ManageItemLists.Domain.PackageTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackagAssets.DTOs
{
    public class CreateInvestmentCostAssetsDto
    {
        public Guid InvestmentCostPackageComponentId { get; set; }
        public Guid DevicesAndAssetsUHIAId { get; set; }
        public int Quantity { get; set; }
        public double? YearlyDepreciationPercentage { get; set; }
        public double YearlyMaintenancePercentage { get; set; }
        public double? TotalCost { get; set; }
        public double? YearlyDepreciationCostForTheAddedAssets { get; set; }
        public double? YearlyMaintenanceCostForTheAddedAsset { get; set; }
        public InvestmentCostPackageAsset ToInvestmentCostPackageAsset(string createdBy, string tenantId) => InvestmentCostPackageAsset.Create(null, InvestmentCostPackageComponentId, DevicesAndAssetsUHIAId, Quantity, YearlyDepreciationPercentage, YearlyMaintenancePercentage,
           TotalCost, YearlyDepreciationCostForTheAddedAssets, YearlyMaintenanceCostForTheAddedAsset, createdBy, tenantId);

    }
}
