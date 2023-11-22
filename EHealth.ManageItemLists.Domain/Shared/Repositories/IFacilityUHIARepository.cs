using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using EHealth.ManageItemLists.Domain.Facility.UHIA;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IFacilityUHIARepository
    {
        Task<Guid> Create(FacilityUHIA input);
        Task<bool> Update(FacilityUHIA input);
        Task<bool> Delete(FacilityUHIA input);
        Task<FacilityUHIA?> Get(Guid id);
        Task<PagedResponse<FacilityUHIA>> Search(Expression<Func<FacilityUHIA, bool>> predicate, int pageNumber, int pageSize,bool enablePagination, string? orderBy, bool? ascending);
        Task<bool> IsItemLIstBusy(int itemListId);
    }
}
