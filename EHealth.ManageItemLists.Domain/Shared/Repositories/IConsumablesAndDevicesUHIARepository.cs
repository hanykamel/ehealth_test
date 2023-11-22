using EHealth.ManageItemLists.Domain.ConsumablesAndDevices;
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
    public interface IConsumablesAndDevicesUHIARepository
    {
        Task<Guid> Create(ConsumablesAndDevicesUHIA input);
        Task<bool> Update(ConsumablesAndDevicesUHIA input);
        Task<bool> Delete(ConsumablesAndDevicesUHIA input);
        Task<ConsumablesAndDevicesUHIA?> Get(Guid id);
        Task<PagedResponse<ConsumablesAndDevicesUHIA>> Search(Expression<Func<ConsumablesAndDevicesUHIA, bool>> predicate, int pageNumber, int pageSize,bool enablePagination, string? orderBy, bool? ascending);
        Task<bool> IsItemLIstBusy(int itemListId);
    }
}
