using EHealth.ManageItemLists.Application.FeesOfResourcesPerUnitPackageComponent.Recources.DTOs;
using EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackagAssets.DTOs;
using EHealth.ManageItemLists.Domain.Packages.PackageHeaders;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackagAssets.Queries
{
    public class InvestmentCostCalculateSummaryQuery : InvestmentCostCalculateSummaryDto, IRequest<InvestmentCostSummaryDto>
    {
        private readonly IInvestmentCostPackageComponentRepository _investmentCostPackageComponentRepository;
        public InvestmentCostCalculateSummaryQuery(InvestmentCostCalculateSummaryDto request, IInvestmentCostPackageComponentRepository investmentCostPackageComponentRepository)
        {
            PackageHeaderId = request.PackageHeaderId;
            PriceDate = request.PriceDate;
            _investmentCostPackageComponentRepository = investmentCostPackageComponentRepository;
        }
    }
}
