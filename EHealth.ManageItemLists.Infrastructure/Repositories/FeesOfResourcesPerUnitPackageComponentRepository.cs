using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.Drugs.DrugsUHIA;
using EHealth.ManageItemLists.Domain.Packages.FeesOfResourcesPerUnitPackage.FeesOfResourcesPerUnitPackageComponents;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Infrastructure.Repositories
{
    public class FeesOfResourcesPerUnitPackageComponentRepository : IFeesOfResourcesPerUnitPackageComponentRepository
    {
        private readonly EHealthDbContext _eHealthDbContext;
        public FeesOfResourcesPerUnitPackageComponentRepository(EHealthDbContext eHealthDbContext)
        {
            _eHealthDbContext = eHealthDbContext;
        }
        public async Task<Guid> Create(FeesOfResourcesPerUnitPackageComponent input)
        {
            await _eHealthDbContext.FeesOfResourcesPerUnitPackageComponents.AddAsync(input);
            await _eHealthDbContext.SaveChangesAsync();
            return input.Id;
        }

        public Task<bool> Delete(FeesOfResourcesPerUnitPackageComponent input)
        {
            throw new NotImplementedException();
        }

        public Task<FeesOfResourcesPerUnitPackageComponent?> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResponse<FeesOfResourcesPerUnitPackageComponent>> Search(Expression<Func<FeesOfResourcesPerUnitPackageComponent, bool>> predicate, int pageNumber, int pageSize, bool enablePagination, string? orderBy, bool? ascending)
        {
            var query = _eHealthDbContext.FeesOfResourcesPerUnitPackageComponents
               .OrderByDescending(e => e.Id)
               .Where(predicate)
               .Include(f=>f.FacilityUHIA)
               .Include(x=>x.FeesOfResourcesPerUnitPackageResources).ThenInclude(x=>x.ResourceUHIA).ThenInclude(x=>x.ItemListPrices).ThenInclude(x=>x.PriceUnit)
               .AsQueryable();



            return new PagedResponse<FeesOfResourcesPerUnitPackageComponent>
            {
                Data = enablePagination == true ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() : await query.ToListAsync(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize : await query.CountAsync(),
                TotalCount = await query.CountAsync()
            };
        }

        public async Task<bool> Update(FeesOfResourcesPerUnitPackageComponent input)
        {
            {
                return await _eHealthDbContext.SaveChangesAsync() > 0;
            }
            return false;
        }
    }
}
