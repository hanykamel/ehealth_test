using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.Packages.FeesOfResourcesPerUnitPackage.FeesOfResourcesPerUnitPackageComponents;
using EHealth.ManageItemLists.Domain.Packages.FeesOfResourcesPerUnitPackage.FeesOfResourcesPerUnitPackageResources;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Infrastructure.Repositories
{
    public class FeesOfResourcesPerUnitPackageResourceRepository : IFeesOfResourcesPerUnitPackageResourceRepository
    {
        private readonly EHealthDbContext _eHealthDbContext;
        public FeesOfResourcesPerUnitPackageResourceRepository(EHealthDbContext eHealthDbContext)
        {
            _eHealthDbContext = eHealthDbContext;
        }
        public async Task<Guid> Create(FeesOfResourcesPerUnitPackageResource input)
        {
            await _eHealthDbContext.FeesOfResourcesPerUnitPackageResources.AddAsync(input);
            await _eHealthDbContext.SaveChangesAsync();
            return input.Id;
        }

        public Task<bool> Delete(FeesOfResourcesPerUnitPackageResource input)
        {
            throw new NotImplementedException();
        }

        public async Task<FeesOfResourcesPerUnitPackageResource?> Get(Guid id)
        {
            return await _eHealthDbContext.FeesOfResourcesPerUnitPackageResources
                .Include(f => f.ResourceUHIA)
                .ThenInclude(f=>f.ItemListPrices).ThenInclude(f=>f.PriceUnit)
                .FirstOrDefaultAsync(x => x.Id == id
                //&& x.IsDeleted != true
                );
        }

 
        public async Task<PagedResponse<FeesOfResourcesPerUnitPackageResource>> Search(Expression<Func<FeesOfResourcesPerUnitPackageResource, bool>> predicate, int pageNumber, int pageSize, bool enablePagination, string? orderBy, bool? ascending)
        {
            var query = _eHealthDbContext.FeesOfResourcesPerUnitPackageResources
               .OrderByDescending(e => e.Id)
               .Where(predicate)
               .AsQueryable();



            return new PagedResponse<FeesOfResourcesPerUnitPackageResource>
            {
                Data = enablePagination == true ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() : await query.ToListAsync(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize : await query.CountAsync(),
                TotalCount = await query.CountAsync()
            };
        }
        public async Task<bool> Update(FeesOfResourcesPerUnitPackageResource input)
        {
            {
                return await _eHealthDbContext.SaveChangesAsync() > 0;
            }
            return false;
        }
    }
}
