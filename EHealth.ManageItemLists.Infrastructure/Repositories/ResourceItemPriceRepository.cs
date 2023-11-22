using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.ItemListPricing;
using EHealth.ManageItemLists.Domain.Resource.ItemPrice;
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
    public class ResourceItemPriceRepository : IResourceItemPriceRepository
    {
        private readonly EHealthDbContext _eHealthDbContext;
        public ResourceItemPriceRepository(EHealthDbContext eHealthDbContext)
        {
            _eHealthDbContext = eHealthDbContext;
        }

        public async Task<int> Create(ResourceItemPrice input)
        {
            await _eHealthDbContext.ResourceItemPrices.AddAsync(input);
            await _eHealthDbContext.SaveChangesAsync();
            return input.Id;
        }

        public async Task<bool> CreateRange(List<ResourceItemPrice> input)
        {
            await _eHealthDbContext.ResourceItemPrices.AddRangeAsync(input);
            await _eHealthDbContext.SaveChangesAsync();
            return true;
        }
        public Task<bool> Delete(ResourceItemPrice input)
        {
            throw new NotImplementedException();
        }

        public Task<ResourceItemPrice?> Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResponse<ResourceItemPrice>> Search(Expression<Func<ResourceItemPrice, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            var query = _eHealthDbContext.ResourceItemPrices.Include(f=>f.PriceUnit).Where(predicate)
              .AsQueryable();

            query = query.OrderByDescending(x => x.ModifiedOn != null ? x.ModifiedOn : x.CreatedOn);
            return new PagedResponse<ResourceItemPrice>
            {
                TotalCount = await query.CountAsync(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize : await query.CountAsync(),
                Data = enablePagination == true ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() : await query.ToListAsync()
            };
        }

        public Task<bool> Update(ResourceItemPrice input)
        {
            throw new NotImplementedException();
        }
    }
}
