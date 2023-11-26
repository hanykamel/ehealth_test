using EHealth.ManageItemLists.Application.FeesOfResourcesPerUnitPackageComponent.Recources.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;

namespace EHealth.ManageItemLists.Application.FeesOfResourcesPerUnitPackageComponent.Recources.Queries
{
    public class FeesOfResourceCalculateSummaryQuery : FeesOfResourceCalculateSummaryDto, IRequest<FeesOfResourceSummaryDto>
    {
        private readonly IFeesOfResourcesPerUnitPackageComponentRepository _feesOfResourcesPerUnitPackageComponentRepository;
        public FeesOfResourceCalculateSummaryQuery(FeesOfResourceCalculateSummaryDto request, IFeesOfResourcesPerUnitPackageComponentRepository feesOfResourcesPerUnitPackageComponentRepository)
        {
            PackageHeaderId = request.PackageHeaderId;
            PriceDate=request.PriceDate;    
            _feesOfResourcesPerUnitPackageComponentRepository = feesOfResourcesPerUnitPackageComponentRepository;
        }
    }
}
