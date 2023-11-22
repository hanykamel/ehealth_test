using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.LocalTypeOfMeasure;
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
    public class UnitOfMeasureRepository : IUnitOfMeasureRepository
    {
        private readonly EHealthDbContext _eHealthDbContext;
        public UnitOfMeasureRepository(EHealthDbContext eHealthDbContext)
        {
            _eHealthDbContext = eHealthDbContext;
        }

        public Task<int> CreateUnitOfMeasure(UnitOfMeasure input)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUnitOfMeasure(UnitOfMeasure input)
        {
            throw new NotImplementedException();
        }

        public Task<UnitOfMeasure?> Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResponse<UnitOfMeasure>> Search(Expression<Func<UnitOfMeasure, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            var query = _eHealthDbContext.UnitOfMeasures.Where(predicate).AsQueryable();

            query = query.OrderBy(x => x.MeasureTypeENG);
            return new PagedResponse<UnitOfMeasure>
            {
                TotalCount = await query.CountAsync(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize : await query.CountAsync(),
                Data = enablePagination == true ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() : await query.ToListAsync()
            };
        }

        public Task<bool> UpdateUnitOfMeasure(UnitOfMeasure input)
        {
            throw new NotImplementedException();
        }
    }
}
