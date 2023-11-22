using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.DoctorFees.UHIA;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Infrastructure.Repositories
{
    public class DoctorFeesUHIARepository : IDoctorFeesUHIARepository
    {
        private readonly EHealthDbContext _eHealthDbContext;
        public DoctorFeesUHIARepository(EHealthDbContext eHealthDbContext)
        {
            _eHealthDbContext = eHealthDbContext;
        }
        public async Task<Guid> Create(DoctorFeesUHIA input)
        {
            await _eHealthDbContext.DoctorFeesUHIA.AddAsync(input);
            await _eHealthDbContext.SaveChangesAsync();
            return input.Id;
        }

        public async Task<bool> Delete(DoctorFeesUHIA input)
        {
            return await _eHealthDbContext.SaveChangesAsync() > 0;
        }

        public async Task<DoctorFeesUHIA?> Get(Guid id)
        {
            return await _eHealthDbContext.DoctorFeesUHIA
                .Include(f => f.ItemListPrices
                //.Where(p => p.IsDeleted != true)
                ).ThenInclude(p => p.UnitOfDoctorFees)
                .Include(f => f.PackageComplexityClassification)
                .FirstOrDefaultAsync(x => x.Id == id 
                //&& x.IsDeleted != true
                );
        }

        public async Task<PagedResponse<DoctorFeesUHIA>> Search(Expression<Func<DoctorFeesUHIA, bool>> predicate, int pageNumber, int pageSize, bool enablePagination, string? orderBy, bool? ascending)
        {
            //var query = _eHealthDbContext.DoctorFeesUHIA.Where(predicate)
            //    .Include(f => f.PackageComplexityClassification)                
            //    .Include(f=>f.ItemListPrices.OrderByDescending(e => e.EffectiveDateFrom)).ThenInclude(f=>f.UnitOfDoctorFees)
            //    .AsQueryable();
            //query = query.OrderByDescending(x => x.CreatedOn);
            var query = _eHealthDbContext.DoctorFeesUHIA.Where(predicate)
               .Include(f => f.PackageComplexityClassification)
               .Include(f => f.ItemListPrices
               //.Where(x => x.IsDeleted == false)
               .OrderByDescending(e => e.EffectiveDateFrom)).ThenInclude(f => f.UnitOfDoctorFees)
               .AsQueryable();

            if (!string.IsNullOrEmpty(orderBy))
                switch (orderBy.ToLower())
                {
                    case "code":
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

                case "complexityclassificationcode":
                    if (ascending == false)
                        query = query.OrderByDescending(x => x.PackageComplexityClassification.Code);
                    else
                        query = query.OrderBy(x => x.PackageComplexityClassification.Code);
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

                case "doctorsfees":
                    if (ascending == false)
                        query = query.OrderByDescending(x => x.ItemListPrices.FirstOrDefault().DoctorFees);
                    else
                        query = query.OrderBy(x => x.ItemListPrices.FirstOrDefault().DoctorFees);
                    break;

                case "unitdoctorfees":
                    if (ascending == false)
                        query = query.OrderByDescending(x => x.ItemListPrices.FirstOrDefault().UnitOfDoctorFees.NameEN);
                    else
                        query = query.OrderBy(x => x.ItemListPrices.FirstOrDefault().UnitOfDoctorFees.NameEN);
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

            return new PagedResponse<DoctorFeesUHIA>
            {
                TotalCount = await query.CountAsync(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize : await query.CountAsync(),
                Data = enablePagination == true ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() : await query.ToListAsync()
            };
        }

        public async Task<bool> Update(DoctorFeesUHIA input)
        {
            return await _eHealthDbContext.SaveChangesAsync() > 0;
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
