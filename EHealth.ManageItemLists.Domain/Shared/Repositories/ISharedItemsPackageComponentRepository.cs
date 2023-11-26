using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackageComponents;
using EHealth.ManageItemLists.Domain.Packages.SharedItemsPackages.SharedItemsPackageComponents;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface ISharedItemsPackageComponentRepository
    {
        Task<Guid> Create(SharedItemsPackageComponent input);
        Task<bool> Update(SharedItemsPackageComponent input);
        Task<bool> Delete(SharedItemsPackageComponent input);
        Task<SharedItemsPackageComponent?> Get(Guid id);
        Task<PagedResponse<SharedItemsPackageComponent>> Search(Expression<Func<SharedItemsPackageComponent, bool>> predicate, int pageNumber, int pageSize, bool enablePagination, string? orderBy, bool? ascending);
    }
}
