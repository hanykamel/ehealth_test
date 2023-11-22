using EHealth.ManageItemLists.Domain.Facility.UHIA;
using EHealth.ManageItemLists.Domain.Resource.UHIA;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IResourceUHIARepository
    {
        Task<Guid> Create(ResourceUHIA input);
        Task<bool> Update(ResourceUHIA input);
        Task<bool> Delete(ResourceUHIA input);
        Task<ResourceUHIA?> Get(Guid id);
        Task<PagedResponse<ResourceUHIA>> Search(Expression<Func<ResourceUHIA, bool>> predicate, int pageNumber, int pageSize,bool enablePagination, string? orderBy, bool? ascending);
        Task<bool> IsItemLIstBusy(int itemListId);
    }
}
