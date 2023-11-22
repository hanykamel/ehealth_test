using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IServiceUHIARepository
    {
        Task<Guid> Create(ServiceUHIA input);
        Task<bool> Update(ServiceUHIA input);
        Task<bool> Delete(ServiceUHIA input);
        Task<ServiceUHIA?> Get(Guid id);
        Task<PagedResponse<ServiceUHIA>> Search(Expression<Func<ServiceUHIA, bool>> predicate,int pageNumber, int pageSize, bool enablePagination, string? orderBy, bool? ascending);
        Task<bool> IsItemLIstBusy(int itemListId);
    }
}
