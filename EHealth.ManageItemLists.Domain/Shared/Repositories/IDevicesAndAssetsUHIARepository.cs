using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IDevicesAndAssetsUHIARepository
    {
        Task<Guid> Create(DevicesAndAssetsUHIA input);
        Task<bool> Update(DevicesAndAssetsUHIA input);
        Task<bool> Delete(DevicesAndAssetsUHIA input);
        Task<DevicesAndAssetsUHIA?> Get(Guid id);
        Task<PagedResponse<DevicesAndAssetsUHIA>> Search(Expression<Func<DevicesAndAssetsUHIA, bool>> predicate, int pageNumber, int pageSize,bool enablePagination, string? orderBy, bool? ascending);
        Task<bool> IsItemLIstBusy(int itemListId);
    }
}
