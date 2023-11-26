using EHealth.ManageItemLists.Application.ItemListPrices.DTOs;
using EHealth.ManageItemLists.Domain.ItemListPricing;
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackagAssets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackageComponent.DTOs
{
    public class InvestmentCostPackageAssetsDTO
    {
        public Guid? AssetId { get; set; }
        public string? eHealthCode { get; set; }
        public string? ItemNameAr { get; set; }
        public string? ItemNameEn { get; set; }
        public string? ServiceCategoryAr { get; set; }
        public string? ServiceCategoryEn { get; set; }
        public string? SubCategoryAr { get; set; }
        public string? SubCategoryEn { get; set; }
        public double? Price { get; set; }
        public int? Quantity { get; set; }
        public double? TotalCost { get; set; }
        public double? YearlyDepreciationCostForTheAddedAssets { get; set; }
        public double? YearlyMaintenanceCostForTheAddedAsset { get; set; }

        public double? YearlyDepreciationPercentage { get;  set; }
        public double? YearlyMaintenancePercentage { get;  set; }



        public static InvestmentCostPackageAssetsDTO FromInvestmentCostPackageAsset(InvestmentCostPackageAsset input, DateTime? SearchDate)
        {


            return new InvestmentCostPackageAssetsDTO()
            {
                AssetId = input?.Id,
                eHealthCode = input?.DevicesAndAssetsUHIA?.Code,
                ItemNameAr = input?.DevicesAndAssetsUHIA?.DescriptorAr,
                ItemNameEn = input?.DevicesAndAssetsUHIA?.DescriptorEn,
                ServiceCategoryAr = input?.DevicesAndAssetsUHIA?.Category?.CategoryAr,
                ServiceCategoryEn = input?.DevicesAndAssetsUHIA?.Category?.CategoryEn,
                SubCategoryAr = input?.DevicesAndAssetsUHIA?.SubCategory?.SubCategoryAr,
                SubCategoryEn = input?.DevicesAndAssetsUHIA?.SubCategory?.SubCategoryEn,
                Price = (SearchDate == null)
                ? input?.DevicesAndAssetsUHIA?.ItemListPrices?.OrderByDescending(i => i.EffectiveDateFrom)?.FirstOrDefault()?.Price
                : input?.DevicesAndAssetsUHIA?.ItemListPrices?.FirstOrDefault(a => (a.EffectiveDateFrom) <= SearchDate && (a.EffectiveDateTo ?? DateTime.MaxValue) >= SearchDate)?.Price,
                Quantity = input?.Quantity,
                TotalCost = input?.TotalCost,
                YearlyDepreciationCostForTheAddedAssets = input?.YearlyDepreciationCostForTheAddedAssets,
                YearlyMaintenanceCostForTheAddedAsset = input?.YearlyMaintenanceCostForTheAddedAsset,
                YearlyDepreciationPercentage = input?.YearlyDepreciationPercentage,
                YearlyMaintenancePercentage = input?.YearlyMaintenancePercentage

            };

        }


    }
}
