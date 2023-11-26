using EHealth.ManageItemLists.Domain.Packages.SharedItemsPackages.SharedItemsPackageConsumablesAndDevices;
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
    public interface ISharedItemsPackageConsumableAndDeviceRepository
    {
        Task<Guid> Create(SharedItemsPackageConsumableAndDevice input);
        Task<bool> Update(SharedItemsPackageConsumableAndDevice input);
        Task<bool> Delete(SharedItemsPackageConsumableAndDevice input);
        Task<SharedItemsPackageConsumableAndDevice?> Get(Guid id);
        Task<PagedResponse<SharedItemsPackageConsumableAndDevice>> Search(Expression<Func<SharedItemsPackageConsumableAndDevice, bool>> predicate, int pageNumber, int pageSize, bool enablePagination, string? orderBy, bool? ascending);
    }
}
