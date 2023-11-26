using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.DTOs;
using EHealth.ManageItemLists.Application.FeesOfResourcesPerUnitPackageComponent.Recources.DTOs;
using EHealth.ManageItemLists.Domain.Resource.ItemPrice;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;

namespace EHealth.ManageItemLists.Application.FeesOfResourcesPerUnitPackageComponent.Recources.Queries.Handlers
{
    public class FeesOfResourceCalculateSummaryHandler : IRequestHandler<FeesOfResourceCalculateSummaryQuery, FeesOfResourceSummaryDto>
    {
        private readonly IFeesOfResourcesPerUnitPackageComponentRepository _feesOfResourcesPerUnitPackageComponentRepository;
        public FeesOfResourceCalculateSummaryHandler(IFeesOfResourcesPerUnitPackageComponentRepository feesOfResourcesPerUnitPackageComponentRepository)
        {
            _feesOfResourcesPerUnitPackageComponentRepository = feesOfResourcesPerUnitPackageComponentRepository;
        }
        public async Task<FeesOfResourceSummaryDto> Handle(FeesOfResourceCalculateSummaryQuery request, CancellationToken cancellationToken)
        {
            var feesOfResourcesPerUnitPackageComponent = await Domain.Packages.FeesOfResourcesPerUnitPackage
                .FeesOfResourcesPerUnitPackageComponents.FeesOfResourcesPerUnitPackageComponent.Search(_feesOfResourcesPerUnitPackageComponentRepository, x => x.PackageHeaderId == request.PackageHeaderId &&
                x.IsDeleted != true, 1, 1, false, null, null);

            var feesOfResourcesPerUnitPackageComponentData = feesOfResourcesPerUnitPackageComponent.Data.FirstOrDefault(x => x.IsDeleted is not true);

            if (request.PriceDate != null)
            {

                foreach (var item in feesOfResourcesPerUnitPackageComponentData.FeesOfResourcesPerUnitPackageResources)
                {
                    var priceObject = await item.ResourceUHIA.GetPriceByDate(request.PriceDate);

                    if (priceObject != null)
                    {
                        var dailyCostOfResource = priceObject?.PriceUnit?.ResourceUnitOfCostValue is null || priceObject?.PriceUnit?.ResourceUnitOfCostValue == 0 ? 0: priceObject.Price / priceObject?.PriceUnit?.ResourceUnitOfCostValue;
                        item.SetDailyCostOfTheResource(dailyCostOfResource);
                        item.SetTotalDailyCostOfResourcePerFacility(dailyCostOfResource * item.Quantity);
                    }

                }
            }
            var summary = feesOfResourcesPerUnitPackageComponentData?.CalculateFeesOfResourcesSummary();
            return FeesOfResourceSummaryDto.FromFeesOfResourceSummaryDto(summary);

        }
    }
}
