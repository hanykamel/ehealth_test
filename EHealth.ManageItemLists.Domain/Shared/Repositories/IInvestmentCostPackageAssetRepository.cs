using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackagAssets;
using EHealth.ManageItemLists.Domain.Packages.PackageHeaders;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IInvestmentCostPackageAssetRepository
    {
        Task<Guid> Create(InvestmentCostPackageAsset input);
        Task<bool> Update(InvestmentCostPackageAsset input);
        Task<bool> Delete(InvestmentCostPackageAsset input);
        Task<InvestmentCostPackageAsset?> Get(Guid id);
        Task<PagedResponse<InvestmentCostPackageAsset>> Search(Expression<Func<InvestmentCostPackageAsset, bool>> predicate, int pageNumber, int pageSize, bool enablePagination, string? orderBy, bool? ascending);
    }
}
