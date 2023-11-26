using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.Packages.SharedItemsPackages.SharedItemsPackageDrugs;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Infrastructure.Repositories
{
    public class SharedItemsPackageDrugRepository : ISharedItemsPackageDrugRepository
    {
        private readonly EHealthDbContext _eHealthDbContext;

        public SharedItemsPackageDrugRepository(EHealthDbContext eHealthDbContext)
        {
            _eHealthDbContext = eHealthDbContext;
        }



        public async Task<Guid> Create(SharedItemsPackageDrug input)
        {
            await _eHealthDbContext.SharedItemsPackageDrugs.AddAsync(input);
            await _eHealthDbContext.SaveChangesAsync();
            return input.Id;
        }

        public Task<bool> Delete(SharedItemsPackageDrug input)
        {
            throw new NotImplementedException();
        }

        public async Task<SharedItemsPackageDrug?> Get(Guid id)
        {
            var res = await _eHealthDbContext.SharedItemsPackageDrugs.Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            if (res != null)
                return res;
            throw new DataNotFoundException();
        }

        public async Task<PagedResponse<SharedItemsPackageDrug>> Search(Expression<Func<SharedItemsPackageDrug, bool>> predicate, int pageNumber, int pageSize, bool enablePagination, string? orderBy, bool? ascending)
        {
            var query = _eHealthDbContext.SharedItemsPackageDrugs.Where(predicate).AsQueryable();

            return new PagedResponse<SharedItemsPackageDrug>
            {
                TotalCount = await query.CountAsync(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize : await query.CountAsync(),
                Data = enablePagination == true ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() : await query.ToListAsync()
            };
        }


        public async Task<bool> Update(SharedItemsPackageDrug input)
        {
            if (_eHealthDbContext.ChangeTracker.Entries<SharedItemsPackageDrug>().Any(a => a.State == EntityState.Modified))
            {
                return await _eHealthDbContext.SaveChangesAsync() > 0;
            }
            return false;
        }
    }
}
