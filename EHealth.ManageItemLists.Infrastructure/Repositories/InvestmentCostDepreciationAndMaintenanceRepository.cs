using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostDepreciationsAndMaintenances;
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackageComponents;
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
    public class InvestmentCostDepreciationAndMaintenanceRepository : IInvestmentCostDepreciationAndMaintenanceRepository
    {

        private readonly EHealthDbContext _eHealthDbContext;

        public InvestmentCostDepreciationAndMaintenanceRepository(EHealthDbContext eHealthDbContext)
        {
            _eHealthDbContext = eHealthDbContext;
        }



        public async Task<Guid> Create(InvestmentCostDepreciationAndMaintenance input)
        {
            await _eHealthDbContext.InvestmentCostDepreciationsAndMaintenances.AddAsync(input);
            await _eHealthDbContext.SaveChangesAsync();
            return input.Id;
        }

        public Task<bool> Delete(InvestmentCostDepreciationAndMaintenance input)
        {
            throw new NotImplementedException();
        }

        public async Task<InvestmentCostDepreciationAndMaintenance?> Get(Guid id)
        {
            var res = await _eHealthDbContext.InvestmentCostDepreciationsAndMaintenances.Where(x => x.Id == id)
                .Include(p => p.InvestmentCostPackageComponent)
                .FirstOrDefaultAsync();
                return res;

        }

        public async Task<PagedResponse<InvestmentCostDepreciationAndMaintenance>> Search(Expression<Func<InvestmentCostDepreciationAndMaintenance, bool>> predicate, int pageNumber, int pageSize, bool enablePagination, string? orderBy, bool? ascending)
        {
            var query = _eHealthDbContext.InvestmentCostDepreciationsAndMaintenances.Where(predicate)
                .Include(p => p.InvestmentCostPackageComponent).AsQueryable();
            //if (!string.IsNullOrEmpty(orderBy))
            //{
            //    switch (orderBy.ToLower())
            //    {
            //        case "ehealthcode":
            //            if (ascending == false)
            //                query = query.OrderByDescending(e => e.c);
            //            else
            //                query = query.OrderBy(e => e.EHealthCode);
            //            break;
            //        case "uhiacode":
            //            if (ascending == false)
            //                query = query.OrderByDescending(e => e.UHIACode);
            //            else
            //                query = query.OrderBy(e => e.UHIACode);
            //            break;
            //        case "namear":
            //            if (ascending == false)
            //                query = query.OrderByDescending(e => e.NameAr);
            //            else
            //                query = query.OrderBy(e => e.NameAr);
            //            break;
            //        case "nameen":
            //            if (ascending == false)
            //                query = query.OrderByDescending(e => e.NameEn);
            //            else
            //                query = query.OrderBy(e => e.NameEn);
            //            break;

            //        case "packagetypeid":
            //            if (ascending == false)
            //                query = query.OrderByDescending(e => e.PackageTypeId);
            //            else
            //                query = query.OrderBy(e => e.PackageTypeId);
            //            break;
            //        case "packagesubtypeid":
            //            if (ascending == false)
            //                query = query.OrderByDescending(e => e.PackageSubTypeId);
            //            else
            //                query = query.OrderBy(e => e.PackageSubTypeId);
            //            break;

            //        case "packagecomplexityclassificationid":
            //            if (ascending == false)
            //                query = query.OrderByDescending(e => e.PackageComplexityClassificationId);
            //            else
            //                query = query.OrderBy(e => e.PackageComplexityClassificationId);
            //            break;
            //        case "globalpackagetypeid":
            //            if (ascending == false)
            //                query = query.OrderByDescending(e => e.GlobelPackageTypeId);
            //            else
            //                query = query.OrderBy(e => e.GlobelPackageTypeId);
            //            break;
            //        case "packagespecialityid":
            //            if (ascending == false)
            //                query = query.OrderByDescending(e => e.PackageSpecialtyId);
            //            else
            //                query = query.OrderBy(e => e.PackageSpecialtyId);
            //            break;
            //        case "packageduration":
            //            if (ascending == false)
            //                query = query.OrderByDescending(e => e.PackageDuration);
            //            else
            //                query = query.OrderBy(e => e.PackageDuration);
            //            break;
            //        case "activationdatefrom":
            //            if (ascending == false)
            //                query = query.OrderByDescending(e => e.ActivationDateFrom);
            //            else
            //                query = query.OrderBy(e => e.ActivationDateFrom);
            //            break;
            //        case "activationdateto":
            //            if (ascending == false)
            //                query = query.OrderByDescending(e => e.ActivationDateTo);
            //            else
            //                query = query.OrderBy(e => e.ActivationDateTo);
            //            break;
            //        case "packageprice":
            //            if (ascending == false)
            //                query = query.OrderByDescending(e => e.PackagePrice);
            //            else
            //                query = query.OrderBy(e => e.PackagePrice);
            //            break;
            //        case "packageroundprice":
            //            if (ascending == false)
            //                query = query.OrderByDescending(e => e.PackageRoundPrice);
            //            else
            //                query = query.OrderBy(e => e.PackageRoundPrice);
            //            break;
            //        default:
            //            break;
            //    }
            //}
            //else
            //{
            //    query = query.OrderByDescending(e=>e.ModifiedOn != null? e.ModifiedOn : e.CreatedOn);
            //}
            return new PagedResponse<InvestmentCostDepreciationAndMaintenance>
            {
                TotalCount = await query.CountAsync(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize : await query.CountAsync(),
                Data = enablePagination == true ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() : await query.ToListAsync()
            };
        }


        public async Task<bool> Update(InvestmentCostDepreciationAndMaintenance input)
        {
            if (_eHealthDbContext.ChangeTracker.Entries<InvestmentCostDepreciationAndMaintenance>().Any(a => a.State == EntityState.Modified))
            {
                return await _eHealthDbContext.SaveChangesAsync() > 0;
            }
            return false;
        }

    }

}
