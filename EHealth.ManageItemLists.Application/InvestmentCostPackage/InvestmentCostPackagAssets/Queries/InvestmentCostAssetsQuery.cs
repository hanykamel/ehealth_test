using DocumentFormat.OpenXml.Bibliography;
using EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackagAssets.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;

namespace EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackagAssets.Queries
{
    public class InvestmentCostAssetsQuery : LookupPagedRequerst, IRequest<PagedResponse<InvestmentCostAssetsDto>>
    {
        public Guid  InvestmentCostPackageComponentId { get; set; }
        public DateTime?  SearchDate { get; set; }
        public string? OrderBy { get; set; }
        public bool? Ascending { get; set; }
    }
}
