using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.Facility.UHIA;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
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
    public class FacilityUHIARepository : IFacilityUHIARepository
    {
        private readonly EHealthDbContext _eHealthDbContext;
        public FacilityUHIARepository(EHealthDbContext eHealthDbContext)
        {
            _eHealthDbContext = eHealthDbContext;
        }
        public async Task<Guid> Create(FacilityUHIA input)
        {
            await _eHealthDbContext.FacilityUHIA.AddAsync(input);
            await _eHealthDbContext.SaveChangesAsync();
            return input.Id;
        }

        public async Task<bool> Delete(FacilityUHIA input)
        {
            return await _eHealthDbContext.SaveChangesAsync() > 0;
        }

        public async Task<FacilityUHIA?> Get(Guid id)
        {
            return await _eHealthDbContext.FacilityUHIA.Include(f => f.Category)
                .Include(f => f.SubCategory)
                .FirstOrDefaultAsync(x => x.Id == id 
                //&& x.IsDeleted != true
                );
        }

        public async Task<PagedResponse<FacilityUHIA>> Search(Expression<Func<FacilityUHIA, bool>> predicate, int pageNumber, int pageSize,bool enablePagination, string? orderBy, bool? ascending)
        {
            var query = _eHealthDbContext.FacilityUHIA.Where(predicate)
                .Include(f => f.Category)
                .Include(f => f.SubCategory).AsQueryable();

            if (!string.IsNullOrEmpty(orderBy))
            {
                switch (orderBy.ToLower())
                {
                    case "ehealthcode":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.Code);
                        else
                            query = query.OrderBy(x => x.Code);
                        break;

                    case "descriptoren":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.DescriptorEn);
                        else
                            query = query.OrderBy(x => x.DescriptorEn);
                        break;

                    case "descriptorar":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.DescriptorAr);
                        else
                            query = query.OrderBy(x => x.DescriptorAr);
                        break;

                    case "occupancyrate":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.OccupancyRate);
                        else
                            query = query.OrderBy(x => x.OccupancyRate);
                        break;

                    case "operatingrateinhoursperday":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.OperatingRateInHoursPerDay);
                        else
                            query = query.OrderBy(x => x.OperatingRateInHoursPerDay);
                        break;

                    case "operatingdayspermonth":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.OperatingDaysPerMonth);
                        else
                            query = query.OrderBy(x => x.OperatingDaysPerMonth);
                        break;

                    case "categoryen":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.Category.CategoryEn);
                        else
                            query = query.OrderBy(x => x.Category.CategoryEn);
                        break;

                    case "categoryar":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.Category.CategoryAr);
                        else
                            query = query.OrderBy(x => x.Category.CategoryAr);
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

                    default:
                        break;
                }
            }
            else
                query = query.OrderByDescending(x => x.ModifiedOn != null ? x.ModifiedOn : x.CreatedOn);

            return new PagedResponse<FacilityUHIA>
            {
                TotalCount = await query.CountAsync(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize : await query.CountAsync(),
                Data = enablePagination == true ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() : await query.ToListAsync()
            };
        }

        public async Task<bool> Update(FacilityUHIA input)
        {
            if (_eHealthDbContext.ChangeTracker.Entries<FacilityUHIA>().Any(a => a.State == EntityState.Modified))
            {
                return await _eHealthDbContext.SaveChangesAsync() > 0;
            }
            return false;
        }
        public async Task<bool> IsItemLIstBusy(int itemListId)
        {
            var res = await _eHealthDbContext.ItemLists.Where(x => x.Id == itemListId).FirstOrDefaultAsync();
            if (res != null)
                return res.IsBusy;

            throw new DataNotValidException();
        }
    }
}
