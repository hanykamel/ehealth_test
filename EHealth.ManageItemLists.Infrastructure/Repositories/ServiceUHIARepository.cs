using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.ItemListPricing;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace EHealth.ManageItemLists.Infrastructure.Repositories
{
    public class ServiceUHIARepository : IServiceUHIARepository
    {
        private readonly EHealthDbContext _dbContext;
        public ServiceUHIARepository(EHealthDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Guid> Create(ServiceUHIA input)
        {
            await _dbContext.ServicesUHIA.AddAsync(input);
            await _dbContext.SaveChangesAsync();
            return input.Id;
        }

        public async Task<bool> Delete(ServiceUHIA input)
        {
            {
                return await _dbContext.SaveChangesAsync() > 0;
            }
      
        }

        //public async Task<ServiceUHIA?> Get(Guid id)
        //{
        //    var res = await _dbContext.ServicesUHIA.Where(x => x.Id == id ).FirstOrDefaultAsync();
        //    if (res != null)
        //        return await _dbContext.ServicesUHIA.Where(x => x.Id == id )
        //            .Include(f => f.ServiceCategory)
        //            .Include(f => f.ServiceSubCategory)
        //            .Include(f => f.ItemListPrices)
        //        .FirstAsync();

        //    throw new DataNotValidException();
        //}

        //public async Task<PagedResponse<ServiceUHIA>> Search(Expression<Func<ServiceUHIA, bool>> predicate, int pageNumber, int pageSize,bool enablePagination, string? orderBy, bool? ascending)
        //{
        //    var query = _dbContext.ServicesUHIA.Where(predicate)

        //        .Include(f => f.ServiceCategory)
        //        .Include(f => f.ServiceSubCategory)
        //        .Include(f => f.ItemListPrices.OrderByDescending(e => e.EffectiveDateFrom))
        //        .AsQueryable();
        public async Task<ServiceUHIA?> Get(Guid id)
        {
            var res = await _dbContext.ServicesUHIA.Where(x => x.Id == id 
            //&& x.IsDeleted != true
            ).FirstOrDefaultAsync();
            if (res != null)
                return await _dbContext.ServicesUHIA.Where(x => x.Id == id 
                //&& x.IsDeleted != true
                )
                    .Include(f => f.ServiceCategory)
                    .Include(f => f.ServiceSubCategory)
                    .Include(f => f.ItemList)
                    .Include(f => f.ItemListPrices
                    //.Where(y => y.IsDeleted == false)
                    )
                .FirstAsync();

            throw new DataNotValidException();
        }

        public async Task<PagedResponse<ServiceUHIA>> Search(Expression<Func<ServiceUHIA, bool>> predicate, int pageNumber, int pageSize, bool enablePagination, string? orderBy, bool? ascending)
        {
            var query = _dbContext.ServicesUHIA.Where(predicate)

                .Include(f => f.ServiceCategory)
                .Include(f => f.ServiceSubCategory)
                .Include(f => f.ItemListPrices
                //.Where(y => y.IsDeleted == false)
                .OrderByDescending(e => e.EffectiveDateFrom))
                //.Where(f => f.IsDeleted == false)
                .AsQueryable();

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

                    case "shortdescen":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.ShortDescEn);
                        else
                            query = query.OrderBy(x => x.ShortDescEn);
                        break;

                    case "shortdescar":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.ShortDescAr);
                        else
                            query = query.OrderBy(x => x.ShortDescAr);
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
                            query = query.OrderByDescending(x => x.ServiceSubCategory.SubCategoryEn);
                        else
                            query = query.OrderBy(x => x.ServiceSubCategory.SubCategoryEn);
                        break;

                    case "subcategoryar":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.ServiceSubCategory.SubCategoryAr);
                        else
                            query = query.OrderBy(x => x.ServiceSubCategory.SubCategoryAr);
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

            return new PagedResponse<ServiceUHIA>
            {
                Data = enablePagination == true ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() : await query.ToListAsync(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize : await query.CountAsync(),
                TotalCount = await query.CountAsync()
            };
        }

        public async Task<bool> Update(ServiceUHIA input)
        {
            if (_dbContext.ChangeTracker.Entries<ServiceUHIA>().Any(a => a.State == EntityState.Modified))
            {
                return await _dbContext.SaveChangesAsync() > 0;
            }
            if (_dbContext.ChangeTracker.Entries<ItemListPrice>().Any(a => (a.CurrentValues.ToObject() as ItemListPrice)!.ServiceUHIAId == input.Id))
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
