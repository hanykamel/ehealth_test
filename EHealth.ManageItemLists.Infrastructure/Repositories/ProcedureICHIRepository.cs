using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.ConsumablesAndDevices;
using EHealth.ManageItemLists.Domain.ItemListPricing;
using EHealth.ManageItemLists.Domain.Procedures.ProceduresICHI;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Infrastructure.Repositories
{
    public class ProcedureICHIRepository : IProcedureICHIRepository
    {
        private readonly EHealthDbContext _dbContext;
        public ProcedureICHIRepository(EHealthDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Guid> Create(ProcedureICHI input)
        {
            await _dbContext.ProceduresICHI.AddAsync(input);
            await _dbContext.SaveChangesAsync();
            return input.Id;
        }

        public async Task<bool> Delete(ProcedureICHI input)
        {
            
                return await _dbContext.SaveChangesAsync() > 0;
            
        }

        //public async Task<ProcedureICHI?> Get(Guid id)
        //{
        //    return await _dbContext.ProceduresICHI.Where(x => x.Id == id)
        //         .Include(f => f.ServiceCategory)
        //         .Include(f => f.SubCategory)
        //         .Include(f => f.LocalSpecialtyDepartment)
        //         .Include(f => f.ItemListPrices).FirstAsync();
        //}
        public async Task<ProcedureICHI?> Get(Guid id)
        {
            return await _dbContext.ProceduresICHI.Where(x => x.Id == id 
            //&& x.IsDeleted != true
            )
                 .Include(f => f.ServiceCategory)
                 .Include(f => f.SubCategory)
                 .Include(f => f.LocalSpecialtyDepartment)
                 .Include(f => f.ItemListPrices
                 //.Where(p => p.IsDeleted != true)
                 ).FirstAsync();
        }

        public async Task<PagedResponse<ProcedureICHI>> Search(Expression<Func<ProcedureICHI, bool>> predicate, int pageNumber, int pageSize, bool enablePagination, string? orderBy, bool? ascending)
        {
            var query = _dbContext.ProceduresICHI.Where(predicate)
               .Include(f => f.ServiceCategory)
               .Include(f => f.SubCategory)
               .Include(f => f.LocalSpecialtyDepartment)
               .Include(f => f.ItemListPrices.OrderByDescending(e => e.EffectiveDateFrom))
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

                    case "titleen":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.TitleEn);
                        else
                            query = query.OrderBy(x => x.TitleEn);
                        break;

                    case "titlear":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.TitleAr);
                        else
                            query = query.OrderBy(x => x.TitleAr);
                        break;

                    case "localspecialtydepartment":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.LocalSpecialtyDepartment.LocalSpecialityENG);
                        else
                            query = query.OrderBy(x => x.LocalSpecialtyDepartment.LocalSpecialityENG);
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

            return new PagedResponse<ProcedureICHI>
            {
                Data = enablePagination == true ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() : await query.ToListAsync(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize : await query.CountAsync(),
                TotalCount = await query.CountAsync()
            };
        }

        public async Task<bool> Update(ProcedureICHI input)
        {
            if (_dbContext.ChangeTracker.Entries<ProcedureICHI>().Any(a => a.State == EntityState.Modified))
            {
                return await _dbContext.SaveChangesAsync() > 0;
            }
            if (_dbContext.ChangeTracker.Entries<ItemListPrice>().Any(a => (a.CurrentValues.ToObject() as ItemListPrice)!.ProcedureICHIId == input.Id))
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
