using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.Facility.UHIA;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Infrastructure.Repositories.Lookups
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly EHealthDbContext _eHealthDbContext;
        public CategoriesRepository(EHealthDbContext eHealthDbContext)
        {
            _eHealthDbContext = eHealthDbContext;
        }
        public Task<int> Create(Category input)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Category input)
        {
            throw new NotImplementedException();
        }

        public Task<Category?> Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResponse<Category>> Search(Expression<Func<Category, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            var query = _eHealthDbContext.Categories.Where(predicate)
                .Include(f => f.ItemListSubtype).AsQueryable();

            query = query.OrderBy(x => x.CategoryEn);
            return new PagedResponse<Category>
            {
                TotalCount = await query.CountAsync(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize : await query.CountAsync(),
                Data = enablePagination == true ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() : await query.ToListAsync()
            };
        }

        public Task<bool> Update(Category input)
        {
            throw new NotImplementedException();
        }
    }
}
