using EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackageComponent.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackageComponent.Queries.Handler
{
    using InvestmentCostPackageComponent = EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackageComponents.InvestmentCostPackageComponent;
    public class InvestmentCostPackageAssetsQueryHandler : IRequestHandler<InvestmentCostPackageAssetsQuery, PagedResponse<InvestmentCostPackageAssetsDTO>>
    {
        
        private readonly IInvestmentCostPackageComponentRepository _investmentCostPackageComponentRepository;
        public InvestmentCostPackageAssetsQueryHandler(IInvestmentCostPackageComponentRepository investmentCostPackageComponentRepository)
        {
            _investmentCostPackageComponentRepository = investmentCostPackageComponentRepository;
        }
        public async Task<PagedResponse<InvestmentCostPackageAssetsDTO>> Handle(InvestmentCostPackageAssetsQuery request, CancellationToken cancellationToken)
        {
            var component = await InvestmentCostPackageComponent.Search(_investmentCostPackageComponentRepository, p => p.PackageHeaderId == request.PackageHeaderId, 1, 1, false, null, null);

            if (component == null || component.TotalCount==0)
            {
                return null;
            }

            var investmentCostPackageAssets = await component?.Data?.FirstOrDefault()?.SearchAssets(request.SearchDate, request.PageNumber, request.PageSize, request.EnablePagination, request.OrderBy, request.Ascending);

            return new PagedResponse<InvestmentCostPackageAssetsDTO>()
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = investmentCostPackageAssets.Data.Count,
                Data = investmentCostPackageAssets.Data.Select(i => InvestmentCostPackageAssetsDTO.FromInvestmentCostPackageAsset(i,request.SearchDate)).ToList(),
            };
        }
    }
}
