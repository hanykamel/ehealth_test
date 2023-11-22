using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.ItemListSubtypes;
using EHealth.ManageItemLists.Domain.ItemListTypes;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Sub_Categories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Infrastructure.Repositories.Lookups
{
    public class ItemListSubtypeRepository : IItemListSubtypeRepository
    {
        private readonly EHealthDbContext _eHealthDbContext;
        public ItemListSubtypeRepository(EHealthDbContext eHealthDbContext)
        {
            _eHealthDbContext = eHealthDbContext;
        }
        public async Task<ItemListSubtype> GetById(int id)
        {
            return await _eHealthDbContext.ItemListSubtypes.Include(x => x.ItemListType).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PagedResponse<ItemListSubtype>> Search(int? id, string code, int? itemListTypeId, string? nameAr, string? nameEN, int pageNumber, int pageSize)
        {
            var query = _eHealthDbContext.ItemListSubtypes.Include(x => x.ItemListType).AsQueryable();

            query = query.OrderBy(x => x.NameEN);
            if (!string.IsNullOrEmpty(code))
            {
                query = query.Where(w => w.Code!.Contains(code));
            }

            if (id is not null)
            {
                query = query.Where(w => w.Id == id);
            }

            if (nameAr is not null)
            {
                query = query.Where(w => w.NameAr == nameAr);
            }
            if (nameEN is not null)
            {
                query = query.Where(w => w.NameEN == nameAr);
            }
            if (itemListTypeId is not null)
            {
                query = query.Where(w => w.ItemListTypeId == itemListTypeId);
            }
           

            return new PagedResponse<ItemListSubtype>
            {
                Data = await query.ToListAsync(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = await query.CountAsync()
            };
        }

        public async Task<PagedResponse<ItemListSubtype>> Search(Expression<Func<ItemListSubtype, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            //var query = _eHealthDbContext.ItemListSubtypes.Where(predicate).Include(x => x.ItemListType).AsQueryable();
            var query = _eHealthDbContext.ItemListSubtypes.Where(predicate).Include(f => f.ItemListType).AsQueryable();

            return new PagedResponse<ItemListSubtype>
            {
                Data = enablePagination == true ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() : await query.ToListAsync(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize : await query.CountAsync(),
                TotalCount = await query.CountAsync()
            };
        }
    }
}
