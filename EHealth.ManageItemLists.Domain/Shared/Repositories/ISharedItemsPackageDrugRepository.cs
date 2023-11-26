using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackagAssets;
using EHealth.ManageItemLists.Domain.Packages.SharedItemsPackages.SharedItemsPackageDrugs;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface ISharedItemsPackageDrugRepository
    {
        Task<Guid> Create(SharedItemsPackageDrug input);
        Task<bool> Update(SharedItemsPackageDrug input);
        Task<bool> Delete(SharedItemsPackageDrug input);
        Task<SharedItemsPackageDrug?> Get(Guid id);
        Task<PagedResponse<SharedItemsPackageDrug>> Search(Expression<Func<SharedItemsPackageDrug, bool>> predicate, int pageNumber, int pageSize, bool enablePagination, string? orderBy, bool? ascending);
    }
}
