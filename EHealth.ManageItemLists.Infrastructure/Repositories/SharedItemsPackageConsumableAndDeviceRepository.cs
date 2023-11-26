using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackagAssets;
using EHealth.ManageItemLists.Domain.Packages.SharedItemsPackages.SharedItemsPackageConsumablesAndDevices;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Infrastructure.Repositories
{
    public class SharedItemsPackageConsumableAndDeviceRepository : ISharedItemsPackageConsumableAndDeviceRepository
    {

        private readonly EHealthDbContext _eHealthDbContext;
        public SharedItemsPackageConsumableAndDeviceRepository(EHealthDbContext eHealthDbContext)
        {
            _eHealthDbContext = eHealthDbContext;
        }

        public async Task<Guid> Create(SharedItemsPackageConsumableAndDevice input)
        {
            await _eHealthDbContext.SharedItemsPackageConsumableAndDevices.AddAsync(input);
            await _eHealthDbContext.SaveChangesAsync();
            return input.Id;
        }

        public async Task<bool> Delete(SharedItemsPackageConsumableAndDevice input)
        {
            _eHealthDbContext.SharedItemsPackageConsumableAndDevices.Update(input);
            return await _eHealthDbContext.SaveChangesAsync() > 0;
        }

        public async Task<SharedItemsPackageConsumableAndDevice?> Get(Guid id)
        {
            return await _eHealthDbContext.SharedItemsPackageConsumableAndDevices.Include(x => x.ConsumablesAndDevicesUHIA)
                .Include(x => x.SharedItemsPackageComponent).Include(x => x.Location).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PagedResponse<SharedItemsPackageConsumableAndDevice>> Search(Expression<Func<SharedItemsPackageConsumableAndDevice, bool>> predicate, int pageNumber, int pageSize, bool enablePagination, string? orderBy, bool? ascending)
        {
            var query = _eHealthDbContext.SharedItemsPackageConsumableAndDevices.Where(predicate).Include(x => x.ConsumablesAndDevicesUHIA)
                .Include(x => x.SharedItemsPackageComponent).Include(x => x.Location)
               .AsQueryable();

            query = query.OrderByDescending(x => x.ModifiedOn != null ? x.ModifiedOn : x.CreatedOn);

            return new PagedResponse<SharedItemsPackageConsumableAndDevice>
            {
                TotalCount = await query.CountAsync(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize : await query.CountAsync(),
                Data = enablePagination == true ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() : await query.ToListAsync()
            };
        }

        public async Task<bool> Update(SharedItemsPackageConsumableAndDevice input)
        {
            if (_eHealthDbContext.ChangeTracker.Entries<SharedItemsPackageConsumableAndDevice>().Any(a => a.State == EntityState.Modified))
            {
                return await _eHealthDbContext.SaveChangesAsync() > 0;
            }
            return false;
        }
    }
}
