using EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackagAssets.DTOs;
using EHealth.ManageItemLists.Domain.ConsumablesAndDevices;
using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using EHealth.ManageItemLists.Domain.ItemListPricing;
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackagAssets;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;

namespace EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackagAssets.Queries.Handlers
{
    public class InvestmentCostAssetsQueryHandler : IRequestHandler<InvestmentCostAssetsQuery, PagedResponse<InvestmentCostAssetsDto>>
    {
        private readonly IInvestmentCostPackageAssetRepository _investmentCostPackageAssetRepository;
        public InvestmentCostAssetsQueryHandler(IInvestmentCostPackageAssetRepository investmentCostPackageAssetRepository)
        {
            _investmentCostPackageAssetRepository = investmentCostPackageAssetRepository;
        }
        public async Task<PagedResponse<InvestmentCostAssetsDto>> Handle(InvestmentCostAssetsQuery request, CancellationToken cancellationToken)
        {
            var res = await InvestmentCostPackageAsset.Search(_investmentCostPackageAssetRepository, i => i.InvestmentCostPackageComponentId == request.InvestmentCostPackageComponentId
             , request.PageNo, request.PageSize, request.EnablePagination, request.OrderBy, request.Ascending);
            foreach (var item in res.Data)
            {
                item.DevicesAndAssetsUHIA?.SetPriceByDate(request.SearchDate);
                item.SetTotalCost(item.Quantity * item.DevicesAndAssetsUHIA?.ItemListPrices.Count() == 0 ? 0 : item.DevicesAndAssetsUHIA?.ItemListPrices[0]?.Price);
                var YearlyDepreciationCostForTheAddedAssets = (item.YearlyDepreciationPercentage / 100) * item.TotalCost;
                item.SetYearlyDepreciationCostForTheAddedAssets(item.YearlyDepreciationPercentage is null ? item.TotalCost : YearlyDepreciationCostForTheAddedAssets); ;
                item.SetYearlyMaintenanceCostForTheAddedAsset((item.YearlyMaintenancePercentage / 100) * item.TotalCost);
          
            }
            var data = res.Data.Select(s => InvestmentCostAssetsDto.FromInvestmentCostAssets(s)).ToList();
            return new PagedResponse<InvestmentCostAssetsDto>
            {
                PageNumber = res.PageNumber,
                TotalCount = res.TotalCount,
                PageSize = res.PageSize,
                Data = data
            };
        }


    }
}
