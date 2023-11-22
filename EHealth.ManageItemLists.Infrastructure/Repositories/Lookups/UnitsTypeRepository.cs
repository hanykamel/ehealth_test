using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.LocalSpecialtyDepartments;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.UnitsTypes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Infrastructure.Repositories.Lookups
{
    public class UnitsTypeRepository : IUnitsTypeRepository
    {

        private readonly EHealthDbContext _eHealthDbContext;
        public UnitsTypeRepository(EHealthDbContext eHealthDbContext)
        {
            _eHealthDbContext = eHealthDbContext;
        }

        public Task<int> Create(UnitsType input)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(UnitsType input)
        {
            throw new NotImplementedException();
        }

        public Task<UnitsType?> Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResponse<UnitsType>> Search(Expression<Func<UnitsType, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            var query = _eHealthDbContext.UnitsTypes.Where(predicate).AsQueryable();

            query = query.OrderBy(x => x.UnitEn);
            return new PagedResponse<UnitsType>
            {
                TotalCount = await query.CountAsync(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize : await query.CountAsync(),
                Data = enablePagination == true ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() : await query.ToListAsync()
            };
        }

        public Task<bool> Update(UnitsType input)
        {
            throw new NotImplementedException();
        }
    }
}
