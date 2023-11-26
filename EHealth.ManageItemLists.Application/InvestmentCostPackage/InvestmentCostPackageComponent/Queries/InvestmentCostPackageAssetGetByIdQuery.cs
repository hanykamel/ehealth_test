using EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackageComponent.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;

namespace EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackageComponent.Queries
{
    public class InvestmentCostPackageAssetGetByIdQuery :   IRequest<InvestmentCostPackageAssetsDTO>
    {
        public Guid AssetId { get; set; }

    }
}
