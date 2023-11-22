using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.ItemListSubtypes;
using EHealth.ManageItemLists.Domain.ItemListTypes;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Infrastructure.Repositories.Lookups
{
    public class ItemListTypeRepository : IItemListTypeRepository
    {
        private readonly EHealthDbContext _eHealthDbContext;

        public ItemListTypeRepository(EHealthDbContext eHealthDbContext)
        {
            _eHealthDbContext = eHealthDbContext;
        }

        //public async Task<int> Create(ItemList input)
        //{
        //    await _eHealthDbContext.ItemLists.AddAsync(input);
        //    await _eHealthDbContext.SaveChangesAsync();
        //    return input.Id;
        //}

        //public async Task<bool> Delete(ItemList input)
        //{
        //    _eHealthDbContext.ItemLists.Remove(input);
        //    return await _eHealthDbContext.SaveChangesAsync() > 0;
        //}

        //public async Task<ItemList?> Get(int id)
        //{
        //    return await _eHealthDbContext.ItemLists.FindAsync(id);
        //}

        public async Task<ItemListType> GetById(int id)
        {
            return await _eHealthDbContext.ItemListTypes.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PagedResponse<ItemListType>> Search(Expression<Func<ItemListType, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            var query = _eHealthDbContext.ItemListTypes.Where(predicate).Include(x => x.ItemListSubtypes).AsQueryable();

            query = query.OrderBy(x => x.NameEN);
            return new PagedResponse<ItemListType>
            {
                Data = enablePagination == true ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() : await query.ToListAsync(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize : await query.CountAsync(),
                TotalCount = await query.CountAsync() 
            };
        }

        //public async Task<bool> Update(ItemList input)
        //{
        //    if (_eHealthDbContext.ChangeTracker.Entries<ItemList>().Any(a => a.State == EntityState.Modified))
        //    {
        //        return await _eHealthDbContext.SaveChangesAsync() > 0;
        //    }
        //    return false;
        //}
    }
}
