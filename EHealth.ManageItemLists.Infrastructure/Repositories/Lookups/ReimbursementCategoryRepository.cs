using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.ReimbursementCategories;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Infrastructure.Repositories.Lookups
{
    public class ReimbursementCategoryRepository : IReimbursementCategoryRepository
    {
        private readonly EHealthDbContext _eHealthDbContext;
        public ReimbursementCategoryRepository(EHealthDbContext eHealthDbContext)
        {
            _eHealthDbContext = eHealthDbContext;
        }

        public Task<int> CreateReimbursementCategory(ReimbursementCategory input)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteReimbursementCategory(ReimbursementCategory input)
        {
            throw new NotImplementedException();
        }

        public Task<ReimbursementCategory?> Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResponse<ReimbursementCategory>> Search(Expression<Func<ReimbursementCategory, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            var query = _eHealthDbContext.ReimbursementCategories.Where(predicate).AsQueryable();

            query = query.OrderBy(x => x.NameENG);
            return new PagedResponse<ReimbursementCategory>
            {
                TotalCount = await query.CountAsync(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize : await query.CountAsync(),
                Data = enablePagination == true ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() : await query.ToListAsync()
            };
        }

        public Task<bool> UpdateReimbursementCategory(ReimbursementCategory input)
        {
            throw new NotImplementedException();
        }
    }
}
