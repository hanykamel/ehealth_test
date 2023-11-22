using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.PackageSpecialties;
using EHealth.ManageItemLists.Domain.PackageTypes;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Infrastructure.Repositories.Lookups
{
    public class PackageTypeRepository : IPackageTypeRepository
    {
        private readonly EHealthDbContext _eHealthDbContext;
        public PackageTypeRepository(EHealthDbContext eHealthDbContext)
        {
            _eHealthDbContext = eHealthDbContext;
        }
        public Task<int> Create(PackageType input)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(PackageType input)
        {
            throw new NotImplementedException();
        }
        public Task<PackageType?> Get(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<PagedResponse<PackageType>> Search(Expression<Func<PackageType, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            var query = _eHealthDbContext.PackageTypes.Where(predicate)
                                       .AsQueryable();

            query = query.OrderBy(x => x.NameEN);
            return new PagedResponse<PackageType>
            {
                TotalCount = await query.CountAsync(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize : await query.CountAsync(),
                Data = enablePagination == true ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() : await query.ToListAsync()
            };
        }
        public Task<bool> Update(PackageType input)
        {
            throw new NotImplementedException();
        }
    }
}
