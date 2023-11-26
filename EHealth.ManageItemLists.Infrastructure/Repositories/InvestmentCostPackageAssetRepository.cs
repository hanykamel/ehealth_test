using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackagAssets;
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
    public class InvestmentCostPackageAssetRepository : IInvestmentCostPackageAssetRepository
    {

        private readonly EHealthDbContext _eHealthDbContext;
        public InvestmentCostPackageAssetRepository(EHealthDbContext eHealthDbContext)
        {
            _eHealthDbContext = eHealthDbContext;
        }

        public async Task<Guid> Create(InvestmentCostPackageAsset input)
        {
            await _eHealthDbContext.InvestmentCostPackageAssets.AddAsync(input);
            await _eHealthDbContext.SaveChangesAsync();
            return input.Id;
        }

        public async Task<bool> Delete(InvestmentCostPackageAsset input)
        {
            _eHealthDbContext.InvestmentCostPackageAssets.Update(input);
            return await _eHealthDbContext.SaveChangesAsync() > 0;
        }

        public async Task<InvestmentCostPackageAsset?> Get(Guid id)
        {
            return await _eHealthDbContext.InvestmentCostPackageAssets.Include(x => x.DevicesAndAssetsUHIA).Include(x => x.InvestmentCostPackageComponent).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PagedResponse<InvestmentCostPackageAsset>> Search(Expression<Func<InvestmentCostPackageAsset, bool>> predicate, int pageNumber, int pageSize, bool enablePagination, string? orderBy, bool? ascending)
        {
            var query = _eHealthDbContext.InvestmentCostPackageAssets.Where(predicate).Include(f => f.DevicesAndAssetsUHIA).ThenInclude(f=>f.ItemListPrices)
               .Include(f => f.InvestmentCostPackageComponent)
               .AsQueryable();

            query = query.OrderByDescending(x => x.ModifiedOn != null ? x.ModifiedOn : x.CreatedOn);

            return new PagedResponse<InvestmentCostPackageAsset>
            {
                TotalCount = await query.CountAsync(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize : await query.CountAsync(),
                Data = enablePagination == true ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() : await query.ToListAsync()
            };
        }

        public async Task<bool> Update(InvestmentCostPackageAsset input)
        {
            if (_eHealthDbContext.ChangeTracker.Entries<InvestmentCostPackageAsset>().Any(a => a.State == EntityState.Modified))
            {
                return await _eHealthDbContext.SaveChangesAsync() > 0;
            }
            return false;
        }
    }
}
