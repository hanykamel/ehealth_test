using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.GlobelPackageTypes;
using EHealth.ManageItemLists.Domain.PackageSpecialties;
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
    public class GlobalPackageTypeRepository : IGlobelPackageTypeRepository
    {
        private readonly EHealthDbContext _eHealthDbContext;

        public GlobalPackageTypeRepository(EHealthDbContext eHealthDbContext)
        {
            _eHealthDbContext = eHealthDbContext;
        }
        public Task<int> Create(GlobelPackageType input)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(GlobelPackageType input)
        {
            throw new NotImplementedException();
        }

        public async Task<GlobelPackageType?> Get(int id)
        {
            return await _eHealthDbContext.GlobelPackageTypes.FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<PagedResponse<GlobelPackageType>> Search(Expression<Func<GlobelPackageType, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            var query = _eHealthDbContext.GlobelPackageTypes.Where(predicate)
                           .AsQueryable();

            query = query.OrderBy(x => x.GlobalTypeEn);
            return new PagedResponse<GlobelPackageType>
            {
                TotalCount = await query.CountAsync(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize : await query.CountAsync(),
                Data = enablePagination == true ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() : await query.ToListAsync()
            };
        }

        public Task<bool> Update(GlobelPackageType input)
        {
            throw new NotImplementedException();
        }
    }
}
