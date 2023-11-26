using EHealth.ManageItemLists.Application.Facility.UHIA.DTOs;
using EHealth.ManageItemLists.Application.ItemListPrices.DTOs;
using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.SubCategories.DTOs;
using EHealth.ManageItemLists.Domain.Facility.UHIA;
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackagAssets;

namespace EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackagAssets.DTOs
{
    public class InvestmentCostAssetsDto
    {
        public Guid? Id { get; set; }
        public string EHealthCode { get; set; }
        public string ItemName { get; set; }
        public CategoryDto ServiceCategory { get; set; }
        public SubCategoryDto SubCategory { get; set; }
        public double? Price { get; set; }
        public double? TotalCost { get; private set; }
        public double? YearlyDepreciationCostForTheAddedAssets { get; private set; }
        public double? YearlyMaintenanceCostForTheAddedAsset { get; private set; }
        public static  InvestmentCostAssetsDto FromInvestmentCostAssets(InvestmentCostPackageAsset input) =>
    new InvestmentCostAssetsDto
    {
        Id = input.Id,
        //EHealthCode = input.,
        ItemName = input.DevicesAndAssetsUHIA?.DescriptorEn ?? "",
        ServiceCategory = CategoryDto.FromCategory(input.DevicesAndAssetsUHIA?.Category),
        SubCategory = SubCategoryDto.FromSubCategory(input.DevicesAndAssetsUHIA?.SubCategory),
        Price = ItemListPriceDto.FromItemListPrice(input.DevicesAndAssetsUHIA?.ItemListPrices?.OrderByDescending(e => e.EffectiveDateFrom).FirstOrDefault())?.Price,
        TotalCost = input.TotalCost,
        YearlyDepreciationCostForTheAddedAssets = input.YearlyDepreciationCostForTheAddedAssets,
        YearlyMaintenanceCostForTheAddedAsset = input.YearlyMaintenanceCostForTheAddedAsset
    };
    }
}

