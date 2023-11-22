using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Sub_Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface ISubCategoriesRepository
    {
        Task<int> Create(SubCategory input);
        Task<bool> Update(SubCategory input);
        Task<bool> Delete(SubCategory input);
        Task<SubCategory?> Get(int id);
        Task<PagedResponse<SubCategory>> Search(Expression<Func<SubCategory, bool>> predicate, int pageNumber, int pageSize, bool enablePagination);

    }
}
