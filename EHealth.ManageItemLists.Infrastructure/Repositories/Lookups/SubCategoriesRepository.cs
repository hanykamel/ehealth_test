using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Sub_Categories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Infrastructure.Repositories.Lookups
{
    public class SubCategoriesRepository : ISubCategoriesRepository
    {
        private readonly EHealthDbContext _eHealthDbContext;
        public SubCategoriesRepository(EHealthDbContext eHealthDbContext)
        {
            _eHealthDbContext = eHealthDbContext;
        }
        public Task<int> Create(SubCategory input)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(SubCategory input)
        {
            throw new NotImplementedException();
        }

        public Task<SubCategory?> Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResponse<SubCategory>> Search(Expression<Func<SubCategory, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            var query = _eHealthDbContext.SubCategories.Where(predicate)
                .Include(f => f.ItemListSubtype).Include(f => f.Category).AsQueryable();

            query = query.OrderBy(x => x.SubCategoryEn);
            return new PagedResponse<SubCategory>
            {
                TotalCount = await query.CountAsync(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize : await query.CountAsync(),
                Data = enablePagination == true ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() : await query.ToListAsync()
            };
        }

        public Task<bool> Update(SubCategory input)
        {
            throw new NotImplementedException();
        }
    }
}
