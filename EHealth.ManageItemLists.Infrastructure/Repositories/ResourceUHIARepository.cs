using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using EHealth.ManageItemLists.Domain.ItemListPricing;
using EHealth.ManageItemLists.Domain.Resource.ItemPrice;
using EHealth.ManageItemLists.Domain.Resource.UHIA;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
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
    public class ResourceUHIARepository : IResourceUHIARepository
    {
        private readonly EHealthDbContext _eHealthDbContext;
        public ResourceUHIARepository(EHealthDbContext eHealthDbContext)
        {
            _eHealthDbContext = eHealthDbContext;
        }
        public async Task<Guid> Create(ResourceUHIA input)
        {
            await _eHealthDbContext.ResourceUHIA.AddAsync(input);
            await _eHealthDbContext.SaveChangesAsync();
            return input.Id;
        }

        public async Task<bool> Delete(ResourceUHIA input)
        {
            return await _eHealthDbContext.SaveChangesAsync() > 0;
        }

        public async Task<ResourceUHIA?> Get(Guid id)
        {
            return await _eHealthDbContext.ResourceUHIA
                .Include(f => f.Category)
                .Include(f => f.SubCategory)
                .Include(f => f.ItemListPrices
                //.Where(p => p.IsDeleted != true)
                ).ThenInclude(p => p.PriceUnit)
                .FirstOrDefaultAsync(x => x.Id == id 
                //&& x.IsDeleted != true
                );
        }

        public async Task<PagedResponse<ResourceUHIA>> Search(Expression<Func<ResourceUHIA, bool>> predicate, int pageNumber, int pageSize, bool enablePagination, string? orderBy, bool? ascending)
        {
            //var query = _eHealthDbContext.ResourceUHIA.Where(predicate)
            //    .Include(f => f.Category)
            //    .Include(f => f.SubCategory)

            //    .Include(f => f.ItemListPrices.OrderByDescending(e => e.EffectiveDateFrom)).ThenInclude(f => f.PriceUnit)
            //    .AsQueryable();
            var query = _eHealthDbContext.ResourceUHIA.Where(predicate)
                .Include(f => f.Category)
                .Include(f => f.SubCategory)

                .Include(f => f.ItemListPrices
                //.Where(x => x.IsDeleted == false)
                .OrderByDescending(e => e.EffectiveDateFrom)).ThenInclude(f => f.PriceUnit)
                //.Where(f => f.IsDeleted == false)
                .AsQueryable();

            if (!string.IsNullOrEmpty(orderBy))
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

                    case "price":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.ItemListPrices.FirstOrDefault().Price);
                        else
                            query = query.OrderBy(x => x.ItemListPrices.FirstOrDefault().Price);
                        break;

                    case "unitprice":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.ItemListPrices.FirstOrDefault().PriceUnit);
                        else
                            query = query.OrderBy(x => x.ItemListPrices.FirstOrDefault().PriceUnit);
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

            return new PagedResponse<ResourceUHIA>
            {
                TotalCount = await query.CountAsync(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize : await query.CountAsync(),
                Data = enablePagination == true ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() : await query.ToListAsync()
            };
        }

        public async Task<bool> Update(ResourceUHIA input)
        {
            if (_eHealthDbContext.ChangeTracker.Entries<ResourceUHIA>().Any(a => a.State == EntityState.Modified))
            {
                return await _eHealthDbContext.SaveChangesAsync() > 0;
            }
            if (_eHealthDbContext.ChangeTracker.Entries<ResourceItemPrice>().Any(a => (a.CurrentValues.ToObject() as ResourceItemPrice)!.ResourceUHIAId == input.Id))
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
