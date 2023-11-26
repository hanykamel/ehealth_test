using EHealth.ManageItemLists.Application.FeesOfResourcesPerUnitPackageComponent.Recources.DTOs;
using EHealth.ManageItemLists.Application.FeesOfResourcesPerUnitPackageComponent.Recources.Queries;
using EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackagAssets.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Identity;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;
using OfficeOpenXml.ThreadedComments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackagAssets.Queries.Handlers
{
    public class InvestmentCostCalculateSummaryQueryHandler : IRequestHandler<InvestmentCostCalculateSummaryQuery, InvestmentCostSummaryDto>
    {
        private readonly IInvestmentCostPackageComponentRepository _investmentCostPackageComponentRepository;
        private readonly IIdentityProvider _identityProvider;
        public InvestmentCostCalculateSummaryQueryHandler(IInvestmentCostPackageComponentRepository investmentCostPackageComponentRepository, IIdentityProvider identityProvider)
        {
            _investmentCostPackageComponentRepository = investmentCostPackageComponentRepository;
            _identityProvider = identityProvider;
        }
        public async Task<InvestmentCostSummaryDto> Handle(InvestmentCostCalculateSummaryQuery request, CancellationToken cancellationToken)
        {
            var investmentCostPackageComponent = await Domain.Packages.InvestmentCostPackage
                .InvestmentCostPackageComponents.InvestmentCostPackageComponent.Search(_investmentCostPackageComponentRepository, x => x.PackageHeaderId == request.PackageHeaderId &&
                x.IsDeleted != true, 1, 1, false, null, null);

            var investmentCostPackageComponentData = investmentCostPackageComponent.Data.FirstOrDefault(x => x.IsDeleted is not true);

            if (request.PriceDate != null)
            {

                foreach (var item in investmentCostPackageComponentData.InvestmentCostPackagAssets)
                {
                    var priceObject = item.DevicesAndAssetsUHIA.GetPriceByDate(request.PriceDate);

                    if (priceObject != null)
                    {
                        var dailyCost = priceObject?.Price == 0 ? 0 : priceObject?.Price / item.Quantity;
                        item.SetTotalCost(dailyCost);
                        item.SetYearlyDepreciationCostForTheAddedAssets(item.YearlyDepreciationPercentage / 100 * dailyCost);
                        item.SetYearlyMaintenanceCostForTheAddedAsset(item.YearlyMaintenancePercentage / 100 * dailyCost);
                    }

                }
            }

            var summary = investmentCostPackageComponentData?.CalculateInvestmentCostPackageSummary(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
            return InvestmentCostSummaryDto.FromInvestmentCostSummaryDto(summary);

        }
    }
}
