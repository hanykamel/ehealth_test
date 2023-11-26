using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.ConsumablesAndDevices;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Infrastructure.Repositories
{
    public class ItemListRepository : IItemListRepository
    {
        private readonly EHealthDbContext _eHealthDbContext;

        public ItemListRepository(EHealthDbContext eHealthDbContext)
        {
            _eHealthDbContext = eHealthDbContext;
        }

        public async Task<int> Create(ItemList input)
        {
            await _eHealthDbContext.ItemLists.AddAsync(input);
            await _eHealthDbContext.SaveChangesAsync();
            return input.Id;
        }

        public async Task<bool> Delete(ItemList input)
        {
            
             _eHealthDbContext.ItemLists.Update(input);
            return await _eHealthDbContext.SaveChangesAsync()>0;
        }




        public async Task<ItemList?> Get(int id)
        {
            return await _eHealthDbContext.ItemLists.Include(x => x.ItemListSubtype).ThenInclude(t => t.ItemListType).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PagedResponse<ItemList>> Search(Expression<Func<ItemList, bool>> predicate, int pageNumber, int pageSize, string? orderBy, bool? ascending, bool enablePagination)
        {
            var query = _eHealthDbContext.ItemLists.Where(predicate)
                .Include(x => x.serviceUHIAlist)
                .Include(x => x.ConsumablesAndDevicesUHIAlist)
                .Include(x => x.ProcedureICHIlist)
                .Include(x => x.DevicesAndAssetsUHIAlist)
                .Include(x => x.FacilityUHIAlist)
                .Include(x => x.ResourceUHIAlist)
                .Include(x => x.DoctorFeesUHIAlist)
                .Include(x => x.DrugUHIAlist)
                .Include(x => x.ItemListSubtype).ThenInclude(t => t.ItemListType).AsQueryable();

            query = query.AsEnumerable().Select(x =>
            {
                x.ItemCounts = x.serviceUHIAlist.Count() > 0 ? x.serviceUHIAlist.Count()
                           : x.ConsumablesAndDevicesUHIAlist.Count() > 0 ? x.ConsumablesAndDevicesUHIAlist.Count()
                           : x.ProcedureICHIlist.Count() > 0 ? x.ProcedureICHIlist.Count()
                           : x.DevicesAndAssetsUHIAlist.Count() > 0 ? x.DevicesAndAssetsUHIAlist.Count()
                           : x.FacilityUHIAlist.Count() > 0 ? x.FacilityUHIAlist.Count()
                           : x.ResourceUHIAlist.Count() > 0 ? x.ResourceUHIAlist.Count()
                           : x.DrugUHIAlist.Count() > 0 ? x.DrugUHIAlist.Count()
                           : x.DoctorFeesUHIAlist.Count() > 0 ? x.DoctorFeesUHIAlist.Count() : 0; return x;
            }).AsQueryable();

            if (!string.IsNullOrEmpty(orderBy))
                switch (orderBy?.ToLower())
                {
                    case "listcode":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.Code);
                        else
                            query = query.OrderBy(x => x.Code);
                        break;

                    case "typeen":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.ItemListSubtype.ItemListType.NameEN).ThenBy(x => x.ItemListSubtype.NameEN);
                        else
                            query = query.OrderBy(x => x.ItemListSubtype.ItemListType.NameEN).ThenBy(x => x.ItemListSubtype.NameEN);
                        break;

                    case "typear":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.ItemListSubtype.ItemListType.NameAr).ThenBy(x => x.ItemListSubtype.NameAr);
                        else
                            query = query.OrderBy(x => x.ItemListSubtype.ItemListType.NameAr).ThenBy(x => x.ItemListSubtype.NameAr);
                        break;

                    case "listnameen":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.NameEN);
                        else
                            query = query.OrderBy(x => x.NameEN);
                        break;

                    case "listnamear":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.NameAr);
                        else
                            query = query.OrderBy(x => x.NameAr);
                        break;

                    case "noofitems":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.ItemCounts);
                        else
                            query = query.OrderBy(x => x.ItemCounts);
                        break;

                    case "lastupdate":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.ModifiedOn);
                        else
                            query = query.OrderBy(x => x.ModifiedOn);
                        break;

                    case "updatedby":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.ModifiedBy);
                        else
                            query = query.OrderBy(x => x.ModifiedBy);
                        break;

                    default:
                        break;
                }
            else
                query = query.OrderByDescending(x => x.ModifiedOn != null ? x.ModifiedOn : x.CreatedOn);

            return new PagedResponse<ItemList>
            {
                TotalCount = query.Count(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize : query.Count(),
                Data = enablePagination == true ? query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList() : query.ToList()
            };
        }

        public async Task<bool> Update(ItemList input)
        {
            if (_eHealthDbContext.ChangeTracker.Entries<ItemList>().Any(a => a.State == EntityState.Modified))
            {
                return await _eHealthDbContext.SaveChangesAsync() > 0;
            }
            return false;
        }
        public async Task<bool> IsListBusy(int Id)
        {
            var res = await _eHealthDbContext.ItemLists.Where(x => x.Id == Id).FirstOrDefaultAsync();
            if (res != null)
                return res.IsBusy;

            throw new DataNotValidException();
        }
    }
}
