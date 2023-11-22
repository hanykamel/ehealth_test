using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.ItemListPricing;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Sub_Categories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Infrastructure.Repositories.Lookups
{
    public class ItemListPriceRepository : IItemListPriceRepository
    {
        private readonly EHealthDbContext _eHealthDbContext;
        public ItemListPriceRepository(EHealthDbContext eHealthDbContext)
        {
            _eHealthDbContext = eHealthDbContext;
        }
     
        public async Task<int> Create(ItemListPrice input)
        {
            await _eHealthDbContext.ItemListPrices.AddAsync(input);
            await _eHealthDbContext.SaveChangesAsync();
            return input.Id;
        }

        public async Task<bool> CreateRange(List<ItemListPrice> input)
        {
            await _eHealthDbContext.ItemListPrices.AddRangeAsync(input);
            await _eHealthDbContext.SaveChangesAsync();
            return true;
        }
        public Task<bool> Delete(ItemListPrice input)
        {
            throw new NotImplementedException();
        }

        public Task<ItemListPrice?> Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResponse<ItemListPrice>> Search(Expression<Func<ItemListPrice, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            var query = _eHealthDbContext.ItemListPrices.Where(predicate)
              .AsQueryable();

            query = query.OrderByDescending(x => x.ModifiedOn != null ? x.ModifiedOn : x.CreatedOn);

            return new PagedResponse<ItemListPrice>
            {
                TotalCount = await query.CountAsync(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize : await query.CountAsync(),
                Data = enablePagination == true ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() : await query.ToListAsync()
            };
        }

        public Task<bool> Update(ItemListPrice input)
        {
            throw new NotImplementedException();
        }
    }
}
