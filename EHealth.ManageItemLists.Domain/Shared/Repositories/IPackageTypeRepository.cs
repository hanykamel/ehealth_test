using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.ConsumablesCodesStatus;
using EHealth.ManageItemLists.Domain.PackageTypes;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IPackageTypeRepository
    {
        Task<int> Create(PackageType input);
        Task<bool> Update(PackageType input);
        Task<bool> Delete(PackageType input);
        Task<PackageType?> Get(int id);
        Task<PagedResponse<PackageType>> Search(Expression<Func<PackageType, bool>> predicate, int pageNumber, int pageSize, bool enablePagination);
    }
}
