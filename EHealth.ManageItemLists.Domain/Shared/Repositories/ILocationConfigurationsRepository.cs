using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.LocationConfigurations;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface ILocationConfigurationsRepository
    {
        Task<int> Create(LocationConfiguration input);
        Task<bool> Update(LocationConfiguration input);
        Task<bool> Delete(LocationConfiguration input);
        Task<LocationConfiguration?> Get(int id);
        Task<PagedResponse<LocationConfiguration>> Search(Expression<Func<LocationConfiguration, bool>> predicate, int pageNumber, int pageSize, bool enablePagination);
    }
}
