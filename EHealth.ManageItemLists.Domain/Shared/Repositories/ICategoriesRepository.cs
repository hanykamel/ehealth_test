using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.Facility.UHIA;
using EHealth.ManageItemLists.Domain.LocalTypeOfMeasure;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface ICategoriesRepository
    {
        Task<int> Create(Category input);
        Task<bool> Update(Category input);
        Task<bool> Delete(Category input);
        Task<Category?> Get(int id);
        Task<PagedResponse<Category>> Search(Expression<Func<Category, bool>> predicate, int pageNumber, int pageSize, bool enablePagination);
    }
}
