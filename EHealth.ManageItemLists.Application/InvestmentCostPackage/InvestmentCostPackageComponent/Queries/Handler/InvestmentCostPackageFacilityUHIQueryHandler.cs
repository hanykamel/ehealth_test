using EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackageComponent.DTOs;
using EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackageComponent.Queries;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;

namespace EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackageComponent.Queries.Handler
{
    using InvestmentCostPackageComponent = Domain.Packages.InvestmentCostPackage.InvestmentCostPackageComponents.InvestmentCostPackageComponent;
    public class InvestmentCostPackageFacilityUHIQueryHandler : IRequestHandler<InvestmentCostPackageFacilityUHIAQuery, InvestmentCostPackageFacilityUHIADTO>
    {
        private readonly IInvestmentCostPackageComponentRepository _investmentCostPackageComponentRepository;
        public InvestmentCostPackageFacilityUHIQueryHandler(IInvestmentCostPackageComponentRepository investmentCostPackageComponentRepository)
        {
            _investmentCostPackageComponentRepository = investmentCostPackageComponentRepository;
        }
        public async Task<InvestmentCostPackageFacilityUHIADTO> Handle(InvestmentCostPackageFacilityUHIAQuery request, CancellationToken cancellationToken)
        {
            var res = await InvestmentCostPackageComponent.Search(_investmentCostPackageComponentRepository, p => p.PackageHeaderId == request.PackageHeaderId, 1, 1, false, null, null);
            if(res.TotalCount == 0)
            {
                return new InvestmentCostPackageFacilityUHIADTO();
            }
            return InvestmentCostPackageFacilityUHIADTO.FromInvestmentCostPackageComponent(res.Data.FirstOrDefault());
        }
    }
}
