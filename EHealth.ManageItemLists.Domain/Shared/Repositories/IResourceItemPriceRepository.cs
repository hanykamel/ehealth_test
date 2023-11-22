using EHealth.ManageItemLists.Domain.Facility.UHIA;
using EHealth.ManageItemLists.Domain.Resource.ItemPrice;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IResourceItemPriceRepository
    {
        Task<int> Create(ResourceItemPrice input);
        Task<bool> Update(ResourceItemPrice input);
        Task<bool> Delete(ResourceItemPrice input);
        Task<ResourceItemPrice?> Get(int id);
        Task<PagedResponse<ResourceItemPrice>> Search(Expression<Func<ResourceItemPrice, bool>> predicate, int pageNumber, int pageSize, bool enablePagination);
    }
}
