using EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackageComponent.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackageComponent.Queries
{
    public class InvestmentCostPackageAssetsQuery :  IRequest<PagedResponse<InvestmentCostPackageAssetsDTO>>
    {
        public Guid PackageHeaderId { get; set; }
        public DateTime? SearchDate { get; set; }
        public string? OrderBy { get; set; }
        public bool? Ascending { get; set; }
        public bool EnablePagination { get; set; } = true;
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
