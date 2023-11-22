using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using EHealth.ManageItemLists.Domain.DoctorFees.UHIA;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IDoctorFeesUHIARepository
    {
        Task<Guid> Create(DoctorFeesUHIA input);
        Task<bool> Update(DoctorFeesUHIA input);
        Task<bool> Delete(DoctorFeesUHIA input);
        Task<DoctorFeesUHIA?> Get(Guid id);
        Task<PagedResponse<DoctorFeesUHIA>> Search(Expression<Func<DoctorFeesUHIA, bool>> predicate, int pageNumber, int pageSize,bool enablePagination, string? orderBy, bool? ascending);
        Task<bool> IsItemLIstBusy(int itemListId);
    }
}
