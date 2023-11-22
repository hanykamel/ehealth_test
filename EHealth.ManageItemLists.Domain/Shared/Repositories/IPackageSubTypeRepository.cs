using EHealth.ManageItemLists.Domain.ItemListSubtypes;
using EHealth.ManageItemLists.Domain.PackageSubTypes;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IPackageSubTypeRepository
    {
        Task<PagedResponse<PackageSubType>> Search(Expression<Func<PackageSubType, bool>> predicate, int pageNumber, int pageSize, bool enablePagination);
        Task<PackageSubType> GetById(int id);
    }
}
