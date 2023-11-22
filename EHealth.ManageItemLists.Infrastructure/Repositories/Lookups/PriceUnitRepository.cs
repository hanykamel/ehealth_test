using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.PriceUnits;
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
    public class PriceUnitRepository : IPriceUnitRepository
    {
        private readonly EHealthDbContext _eHealthDbContext;
        public PriceUnitRepository(EHealthDbContext eHealthDbContext)
        {
            _eHealthDbContext = eHealthDbContext;
        }
        public Task<int> CreatePriceUnit(PriceUnit input)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeletePriceUnit(PriceUnit input)
        {
            throw new NotImplementedException();
        }

        public Task<PriceUnit?> Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResponse<PriceUnit>> Search(Expression<Func<PriceUnit, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            var query = _eHealthDbContext.PriceUnits.Where(predicate)
                .AsQueryable();

            query = query.OrderBy(x => x.NameEN);
            return new PagedResponse<PriceUnit>
            {
                TotalCount = await query.CountAsync(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize : await query.CountAsync(),
                Data = enablePagination == true ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() : await query.ToListAsync()
            };
        }

        public Task<bool> UpdatePriceUnit(PriceUnit input)
        {
            throw new NotImplementedException();
        }
    }
}
