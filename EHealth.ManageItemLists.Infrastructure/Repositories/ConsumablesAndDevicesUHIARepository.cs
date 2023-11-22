using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.ConsumablesAndDevices;
using EHealth.ManageItemLists.Domain.ItemListPricing;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Infrastructure.Repositories
{
    public class ConsumablesAndDevicesUHIARepository : IConsumablesAndDevicesUHIARepository
    {
        private readonly EHealthDbContext _dbContext;
        public ConsumablesAndDevicesUHIARepository(EHealthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> Create(ConsumablesAndDevicesUHIA input)
        {
            await _dbContext.ConsumablesAndDevicesUHIA.AddAsync(input);
            await _dbContext.SaveChangesAsync();
            return input.Id;
        }


        public async Task<bool> Delete(ConsumablesAndDevicesUHIA input)
        {
            {
                return await _dbContext.SaveChangesAsync() > 0;
            }
        }

        //public async Task<ConsumablesAndDevicesUHIA?> Get(Guid id)
        //{
        //    return await _dbContext.ConsumablesAndDevicesUHIA.Where(x => x.Id == id)
        //        .Include(f => f.ServiceCategory)
        //        .Include(f => f.SubCategory)
        //        .Include(f => f.UnitOfMeasure)
        //        .Include(f => f.ItemListPrices).FirstAsync();
        //}

        //public async Task<PagedResponse<ConsumablesAndDevicesUHIA>> Search(Expression<Func<ConsumablesAndDevicesUHIA, bool>> predicate, int pageNumber, int pageSize, bool enablePagination, string? orderBy, bool? ascending)
        //{
        //    var query = _dbContext.ConsumablesAndDevicesUHIA.Where(predicate)
        //        .Include(f => f.ServiceCategory)
        //        .Include(f => f.SubCategory).
        //        Include(f => f.ItemListPrices.OrderByDescending(e => e.EffectiveDateFrom)).
        //        Include(f => f.UnitOfMeasure).
        //        AsQueryable();
        //    query = query.OrderByDescending(x => x.CreatedOn);
        public async Task<ConsumablesAndDevicesUHIA?> Get(Guid id)
        {
            return await _dbContext.ConsumablesAndDevicesUHIA.Where(x => x.Id == id 
            //&& x.IsDeleted != true
            )
                .Include(f => f.ServiceCategory)
                .Include(f => f.SubCategory)
                .Include(f => f.UnitOfMeasure)
                .Include(f => f.ItemListPrices
                //.Where(p => p.IsDeleted != true)
                ).FirstAsync();
        }

        public async Task<PagedResponse<ConsumablesAndDevicesUHIA>> Search(Expression<Func<ConsumablesAndDevicesUHIA, bool>> predicate, int pageNumber, int pageSize, bool enablePagination, string? orderBy, bool? ascending)
        {
            var query = _dbContext.ConsumablesAndDevicesUHIA.Where(predicate)
                .Include(f => f.ServiceCategory)
                .Include(f => f.SubCategory).
                Include(f => f.ItemListPrices
                //.Where(y => y.IsDeleted == false)
                .OrderByDescending(e => e.EffectiveDateFrom)).
                Include(f => f.UnitOfMeasure).
                AsQueryable();

            if (!string.IsNullOrEmpty(orderBy))
                switch (orderBy.ToLower())
                {
                    case "ehealthcode":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.EHealthCode);
                        else
                            query = query.OrderBy(x => x.EHealthCode);
                        break;

                    case "uhiaid":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.UHIAId);
                        else
                            query = query.OrderBy(x => x.UHIAId);
                        break;

                    case "shortdescriptionen":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.ShortDescriptorEn);
                        else
                            query = query.OrderBy(x => x.ShortDescriptorEn);
                        break;

                    case "shortdescriptionar":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.ShortDescriptorAr);
                        else
                            query = query.OrderBy(x => x.ShortDescriptorAr);
                        break;

                    case "measuretypeen":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.UnitOfMeasure.MeasureTypeENG);
                        else
                            query = query.OrderBy(x => x.UnitOfMeasure.MeasureTypeENG);
                        break;

                    case "measuretypear":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.UnitOfMeasure.MeasureTypeAr);
                        else
                            query = query.OrderBy(x => x.UnitOfMeasure.MeasureTypeAr);
                        break;

                    case "categoryen":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.ServiceCategory.CategoryEn);
                        else
                            query = query.OrderBy(x => x.ServiceCategory.CategoryEn);
                        break;

                    case "categoryar":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.ServiceCategory.CategoryAr);
                        else
                            query = query.OrderBy(x => x.ServiceCategory.CategoryAr);
                        break;

                    case "subcategoryen":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.SubCategory.SubCategoryEn);
                        else
                            query = query.OrderBy(x => x.SubCategory.SubCategoryEn);
                        break;

                    case "subcategoryar":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.SubCategory.SubCategoryAr);
                        else
                            query = query.OrderBy(x => x.SubCategory.SubCategoryAr);
                        break;

                    case "dataeffectivedatefrom":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.DataEffectiveDateFrom);
                        else
                            query = query.OrderBy(x => x.DataEffectiveDateFrom);
                        break;

                    case "dataeffectivedateto":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.DataEffectiveDateTo);
                        else
                            query = query.OrderBy(x => x.DataEffectiveDateTo);
                        break;

                    case "price":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.ItemListPrices.FirstOrDefault().Price);
                        else
                            query = query.OrderBy(x => x.ItemListPrices.FirstOrDefault().Price);
                        break;

                    case "effectivedatefrom":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.ItemListPrices.FirstOrDefault().EffectiveDateFrom);
                        else
                            query = query.OrderBy(x => x.ItemListPrices.FirstOrDefault().EffectiveDateFrom);
                        break;

                    case "effectivedateto":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.ItemListPrices.FirstOrDefault().EffectiveDateTo);
                        else
                            query = query.OrderBy(x => x.ItemListPrices.FirstOrDefault().EffectiveDateTo);
                        break;

                    default:
                        break;
                }
            else
                query = query.OrderByDescending(x => x.ModifiedOn != null ? x.ModifiedOn : x.CreatedOn);

            return new PagedResponse<ConsumablesAndDevicesUHIA>
            {
                Data = enablePagination == true ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() : await query.ToListAsync(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize : await query.CountAsync(),
                TotalCount = await query.CountAsync()
            };
        }


        public async Task<bool> Update(ConsumablesAndDevicesUHIA input)
        {
            if (_dbContext.ChangeTracker.Entries<ConsumablesAndDevicesUHIA>().Any(a => a.State == EntityState.Modified))
            {
                return await _dbContext.SaveChangesAsync() > 0;
            }
            if (_dbContext.ChangeTracker.Entries<ItemListPrice>().Any(a => (a.CurrentValues.ToObject() as ItemListPrice)!.ConsumablesAndDevicesUHIAId == input.Id))
            {
                return await _dbContext.SaveChangesAsync() > 0;
            }
            return false;
        }
        public async Task<bool> IsItemLIstBusy(int itemListId)
        {
            var res = await _dbContext.ItemLists.Where(x => x.Id == itemListId).FirstOrDefaultAsync();
            if (res != null)
                return res.IsBusy;

            throw new DataNotValidException();
        }
    }
}
