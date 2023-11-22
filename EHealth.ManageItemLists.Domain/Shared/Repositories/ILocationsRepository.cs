using EHealth.ManageItemLists.Domain.ItemListTypes;
using EHealth.ManageItemLists.Domain.LocalSpecialtyDepartments;
using EHealth.ManageItemLists.Domain.Locations;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface ILocationsRepository
    {
        Task<int> Create(Location input);
        Task<bool> Update(Location input);
        Task<bool> Delete(Location input);
        Task<PagedResponse<Location>> Search(Expression<Func<Location, bool>> predicate, int pageNumber, int pageSize, bool enablePagination);
        Task<Location> GetById(int id);
    }
}
