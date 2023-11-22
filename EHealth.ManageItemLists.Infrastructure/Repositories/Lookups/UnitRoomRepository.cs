using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.UnitRooms;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Infrastructure.Repositories.Lookups
{
    public class UnitRoomRepository : IUnitRoomRepository
    {
        private readonly EHealthDbContext _eHealthDbContext;
        public UnitRoomRepository(EHealthDbContext eHealthDbContext)
        {
            _eHealthDbContext = eHealthDbContext;
        }
        public Task<int> CreateUnitRoom(UnitRoom input)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUnitRoom(UnitRoom input)
        {
            throw new NotImplementedException();
        }

        public Task<UnitRoom?> Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResponse<UnitRoom>> Search(IUnitRoomRepository repository, Expression<Func<UnitRoom, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            var query = _eHealthDbContext.UnitRooms.Where(predicate)
                .AsQueryable();

            query = query.OrderBy(x => x.NameEN);
            return new PagedResponse<UnitRoom>
            {
                TotalCount = await query.CountAsync(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize : await query.CountAsync(),
                Data = enablePagination == true ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() : await query.ToListAsync()
            };
        }

        public Task<bool> UpdateUnitRoom(UnitRoom input)
        {
            throw new NotImplementedException();
        }
    }
}
