using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.GlobelPackageTypes;
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackageComponents;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
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
    public class InvestmentCostPackageComponentRepository : IInvestmentCostPackageComponentRepository
    {

        private readonly EHealthDbContext _eHealthDbContext;

        public InvestmentCostPackageComponentRepository(EHealthDbContext eHealthDbContext)
        {
            _eHealthDbContext = eHealthDbContext;
        }



        public async Task<Guid> Create(InvestmentCostPackageComponent input)
        {
            await _eHealthDbContext.InvestmentCostPackageComponents.AddAsync(input);
            await _eHealthDbContext.SaveChangesAsync();
            return input.Id;
        }

        public Task<bool> Delete(InvestmentCostPackageComponent input)
        {
            throw new NotImplementedException();
        }

        public async Task<InvestmentCostPackageComponent?> Get(Guid id)
        {
            var res = await _eHealthDbContext.InvestmentCostPackageComponents.Where(x => x.Id == id)
                .Include(p => p.PackageHeader)
                .Include(p => p.FacilityUHIA)
                .Include(p => p.InvestmentCostDepreciationAndMaintenance)
                .Include(p => p.InvestmentCostPackagAssets)
                .FirstOrDefaultAsync();
            if (res != null)
                return res;
            throw new DataNotFoundException();
        }

        public async Task<PagedResponse<InvestmentCostPackageComponent>> Search(Expression<Func<InvestmentCostPackageComponent, bool>> predicate, int pageNumber, int pageSize, bool enablePagination, string? orderBy, bool? ascending)
        {
            var query = _eHealthDbContext.InvestmentCostPackageComponents.Where(predicate)
                .Include(p => p.PackageHeader)
                .Include(p => p.FacilityUHIA).ThenInclude(h => h.Category)
                .Include(p => p.FacilityUHIA).ThenInclude(h => h.SubCategory)
                .Include(p => p.InvestmentCostDepreciationAndMaintenance)
                .Include(p => p.InvestmentCostPackagAssets)
                .ThenInclude(a => a.DevicesAndAssetsUHIA)
                .ThenInclude(d => d.Category)
                .Include(p => p.InvestmentCostPackagAssets)
                   .ThenInclude(a => a.DevicesAndAssetsUHIA)
                   .ThenInclude(d => d.SubCategory)
                                .Include(p => p.InvestmentCostPackagAssets)
                   .ThenInclude(a => a.DevicesAndAssetsUHIA)
                   .ThenInclude(d => d.ItemListPrices)

                .AsQueryable();

            return new PagedResponse<InvestmentCostPackageComponent>
            {
                TotalCount = await query.CountAsync(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize : await query.CountAsync(),
                Data = enablePagination == true ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() : await query.ToListAsync()
            };
        }


        public async Task<bool> Update(InvestmentCostPackageComponent input)
        {
            if (_eHealthDbContext.ChangeTracker.Entries<InvestmentCostPackageComponent>().Any(a => a.State == EntityState.Modified))
            {
                return await _eHealthDbContext.SaveChangesAsync() > 0;
            }
            return false;
        }

    }
}
