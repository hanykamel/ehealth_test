using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.Drugs.DrugsUHIA;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Infrastructure.Repositories
{
    public class DrugsUHIARepository : IDrugsUHIARepository
    {
        private readonly EHealthDbContext _dbContext;
        public DrugsUHIARepository(EHealthDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Guid> Create(DrugUHIA input)
        {
           
                await _dbContext.DrugsUHIA.AddAsync(input);
                await _dbContext.SaveChangesAsync();
                return input.Id;


           
        }

        public async Task<bool> Delete(DrugUHIA input)
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        //public async Task<DrugUHIA?> Get(Guid id)
        //{
        //    return await _dbContext.DrugsUHIA.Where(x => x.Id == id)
        //        .Include(f => f.DrugPrices)
        //        .Include(x=>x.SubUnit)
        //        .Include(x=>x.RegistrationType)
        //        .Include(x=>x.ReimbursementCategory)
        //        .Include(f => f.DrugsPackageType)
        //        .Include(f => f.MainUnit)
        //        .FirstOrDefaultAsync();
        //}

        //public async Task<PagedResponse<DrugUHIA>> Search(Expression<Func<DrugUHIA, bool>> predicate, int pageNumber, int pageSize,bool enablePagination, string? orderBy, bool? ascending)
        //{
        //    var query = _dbContext.DrugsUHIA
        //        .Include(f => f.MainUnit)
        //        .Include(f => f.SubUnit)
        //        .Include(f => f.ReimbursementCategory)
        //        .Include(f => f.RegistrationType)
        //        .Include(f => f.DrugsPackageType)
        //        .Include(f => f.DrugPrices.OrderByDescending(e => e.EffectiveDateFrom))
        //        .Where(predicate)

        //        .AsQueryable();
        public async Task<DrugUHIA?> Get(Guid id)
        {
            return await _dbContext.DrugsUHIA.Where(x => x.Id == id 
            //&& x.IsDeleted != true
            )
                .Include(f => f.DrugPrices
                //.Where(p => p.IsDeleted != true)
                )
                .Include(x => x.SubUnit)
                .Include(x => x.RegistrationType)
                .Include(x => x.ReimbursementCategory)
                .Include(f => f.DrugsPackageType)
                .Include(f => f.MainUnit)
                .FirstOrDefaultAsync();
        }

        public async Task<PagedResponse<DrugUHIA>> Search(Expression<Func<DrugUHIA, bool>> predicate, int pageNumber, int pageSize, bool enablePagination, string? orderBy, bool? ascending)
        {
            var query = _dbContext.DrugsUHIA
                .Include(f => f.MainUnit)
                .Include(f => f.SubUnit)
                .Include(f => f.ReimbursementCategory)
                .Include(f => f.RegistrationType)
                .Include(f => f.DrugsPackageType)
                .Include(f => f.DrugPrices
                //.Where(y => y.IsDeleted == false)
                .OrderByDescending(e => e.EffectiveDateFrom))
                .Where(predicate)
                //.Where(d => d.IsDeleted != true)

                .AsQueryable();

            if (!string.IsNullOrEmpty(orderBy))
                switch (orderBy.ToLower())
                {
                    case "ehealthdrugcode":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.EHealthDrugCode);
                        else
                            query = query.OrderBy(x => x.EHealthDrugCode);
                        break;

                    case "localdrugcode":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.LocalDrugCode);
                        else
                            query = query.OrderBy(x => x.LocalDrugCode);
                        break;

                    case "internationalnonproprietaryname":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.InternationalNonProprietaryName);
                        else
                            query = query.OrderBy(x => x.InternationalNonProprietaryName);
                        break;

                    case "proprietaryname":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.ProprietaryName);
                        else
                            query = query.OrderBy(x => x.ProprietaryName);
                        break;

                    case "dosageform":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.DosageForm);
                        else
                            query = query.OrderBy(x => x.DosageForm);
                        break;

                    case "routeOfadministration":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.RouteOfAdministration);
                        else
                            query = query.OrderBy(x => x.RouteOfAdministration);
                        break;

                    case "manufacturer":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.Manufacturer);
                        else
                            query = query.OrderBy(x => x.Manufacturer);
                        break;

                    case "marketauthorizationholder":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.MarketAuthorizationHolder);
                        else
                            query = query.OrderBy(x => x.MarketAuthorizationHolder);
                        break;

                    case "drugpackageen":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.RegistrationType.RegistrationTypeENG);
                        else
                            query = query.OrderBy(x => x.RegistrationType.RegistrationTypeENG);
                        break;

                    case "drugpackagear":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.RegistrationType.RegistrationTypeAr);
                        else
                            query = query.OrderBy(x => x.RegistrationType.RegistrationTypeAr);
                        break;

                    case "maintype":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.MainUnit.UnitEn);
                        else
                            query = query.OrderBy(x => x.MainUnit.UnitEn);
                        break;

                    case "numberofmainunit":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.NumberOfMainUnit);
                        else
                            query = query.OrderBy(x => x.NumberOfMainUnit);
                        break;

                    case "subunit":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.SubUnit.UnitEn);
                        else
                            query = query.OrderBy(x => x.SubUnit.UnitEn);
                        break;

                    case "numberofsubunitpermainunit":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.NumberOfSubunitPerMainUnit);
                        else
                            query = query.OrderBy(x => x.NumberOfSubunitPerMainUnit);
                        break;

                    case "totalnumbersubunitsofpack":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.TotalNumberSubunitsOfPack);
                        else
                            query = query.OrderBy(x => x.TotalNumberSubunitsOfPack);
                        break;

                    case "reimbursementcategory":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.ReimbursementCategory.NameENG);
                        else
                            query = query.OrderBy(x => x.ReimbursementCategory.NameENG);
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

                    case "mainunitprice":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.DrugPrices.FirstOrDefault().MainUnitPrice);
                        else
                            query = query.OrderBy(x => x.DrugPrices.FirstOrDefault().MainUnitPrice);
                        break;

                    case "fullpackprice":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.DrugPrices.FirstOrDefault().FullPackPrice);
                        else
                            query = query.OrderBy(x => x.DrugPrices.FirstOrDefault().FullPackPrice);
                        break;

                    case "subunitprice":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.DrugPrices.FirstOrDefault().SubUnitPrice);
                        else
                            query = query.OrderBy(x => x.DrugPrices.FirstOrDefault().SubUnitPrice);
                        break;

                    case "effectivedatefrom":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.DrugPrices.FirstOrDefault().EffectiveDateFrom);
                        else
                            query = query.OrderBy(x => x.DrugPrices.FirstOrDefault().EffectiveDateFrom);
                        break;

                    case "effectivedateto":
                        if (ascending == false)
                            query = query.OrderByDescending(x => x.DrugPrices.FirstOrDefault().EffectiveDateTo);
                        else
                            query = query.OrderBy(x => x.DrugPrices.FirstOrDefault().EffectiveDateTo);
                        break;

                    default:
                        break;
                }
            else
                query = query.OrderByDescending(x => x.ModifiedOn != null ? x.ModifiedOn : x.CreatedOn);

            return new PagedResponse<DrugUHIA>
            {
                Data = enablePagination == true ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() : await query.ToListAsync(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize : await query.CountAsync(),
                TotalCount = await query.CountAsync()
            };
        }

        public async Task<bool> Update(DrugUHIA input)
        {
            //if (_dbContext.ChangeTracker.Entries<DrugUHIA>().Any(a => a.State == EntityState.Modified))
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
