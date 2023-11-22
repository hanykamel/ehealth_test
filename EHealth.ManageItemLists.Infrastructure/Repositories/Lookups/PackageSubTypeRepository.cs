using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.PackageComplexityClassifications;
using EHealth.ManageItemLists.Domain.PackageSubTypes;
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
    public class PackageSubTypeRepository : IPackageSubTypeRepository
    {
        private readonly EHealthDbContext _eHealthDbContext;
        public PackageSubTypeRepository(EHealthDbContext eHealthDbContext)
        {
                _eHealthDbContext = eHealthDbContext;
        }
        public async Task<PackageSubType> GetById(int id)
        {
            return await _eHealthDbContext.PackageSubTypes.FindAsync(id);
        }

        public async Task<PagedResponse<PackageSubType>> Search(Expression<Func<PackageSubType, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            var query = _eHealthDbContext.PackageSubTypes.Where(predicate)
                .AsQueryable();

            query = query.OrderBy(x => x.NameEN);
            return new PagedResponse<PackageSubType>
            {
                TotalCount = await query.CountAsync(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize : await query.CountAsync(),
                Data = enablePagination == true ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() : await query.ToListAsync()
            };
        }
    }
}
