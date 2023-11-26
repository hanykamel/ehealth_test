using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.Packages.SharedItemsPackages.SharedItemsPackageComponents;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Infrastructure.Repositories
{
    public class SharedItemsPackageComponentRepository : ISharedItemsPackageComponentRepository
    {
        private readonly EHealthDbContext _eHealthDbContext;

        public SharedItemsPackageComponentRepository(EHealthDbContext eHealthDbContext)
        {
            _eHealthDbContext = eHealthDbContext;
        }



        public async Task<Guid> Create(SharedItemsPackageComponent input)
        {
            await _eHealthDbContext.SharedItemsPackageComponents.AddAsync(input);
            await _eHealthDbContext.SaveChangesAsync();
            return input.Id;
        }

        public Task<bool> Delete(SharedItemsPackageComponent input)
        {
            throw new NotImplementedException();
        }

        public async Task<SharedItemsPackageComponent?> Get(Guid id)
        {
            var res = await _eHealthDbContext.SharedItemsPackageComponents.Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            if (res != null)
                return res;
            throw new DataNotFoundException();
        }

        public async Task<PagedResponse<SharedItemsPackageComponent>> Search(Expression<Func<SharedItemsPackageComponent, bool>> predicate, int pageNumber, int pageSize, bool enablePagination, string? orderBy, bool? ascending)
        {
            var query = _eHealthDbContext.SharedItemsPackageComponents.Where(predicate).AsQueryable();

            return new PagedResponse<SharedItemsPackageComponent>
            {
                TotalCount = await query.CountAsync(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize : await query.CountAsync(),
                Data = enablePagination == true ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() : await query.ToListAsync()
            };
        }


        public async Task<bool> Update(SharedItemsPackageComponent input)
        {
            if (_eHealthDbContext.ChangeTracker.Entries<SharedItemsPackageComponent>().Any(a => a.State == EntityState.Modified))
            {
                return await _eHealthDbContext.SaveChangesAsync() > 0;
            }
            return false;
        }
    }
}
