using EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackageComponent.DTOs;
using MediatR;

namespace EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackageComponent.Queries
{
    public class InvestmentCostPackageFacilityUHIAQuery : IRequest<InvestmentCostPackageFacilityUHIADTO>
    {
        public Guid PackageHeaderId { get; set; }
    }
}
