using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.Locations;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Infrastructure.Repositories
{
    public class LocationsRepository : ILocationsRepository
    {
        private readonly EHealthDbContext _eHealthDbContext;

        public LocationsRepository(EHealthDbContext eHealthDbContext)
        {
            _eHealthDbContext = eHealthDbContext;
        }



        public async Task<int> Create(Location input)
        {
            await _eHealthDbContext.Locations.AddAsync(input);
            await _eHealthDbContext.SaveChangesAsync();
            return input.Id;
        }

        public Task<bool> Delete(Location input)
        {
            throw new NotImplementedException();
        }

        public async Task<Location?> Get(int id)
        {
            var res = await _eHealthDbContext.Locations.Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            if (res != null)
                return res;
            throw new DataNotFoundException();
        }

        public Task<Location> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResponse<Location>> Search(Expression<Func<Location, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            var query = _eHealthDbContext.Locations.Where(predicate).AsQueryable();

            return new PagedResponse<Location>
            {
                TotalCount = await query.CountAsync(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize : await query.CountAsync(),
                Data = enablePagination == true ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() : await query.ToListAsync()
            };
        }

        public async Task<bool> Update(Location input)
        {
            if (_eHealthDbContext.ChangeTracker.Entries<Location>().Any(a => a.State == EntityState.Modified))
            {
                return await _eHealthDbContext.SaveChangesAsync() > 0;
            }
            return false;
        }
    }
}
