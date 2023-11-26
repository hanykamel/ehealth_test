using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.Packages.PackageHeaders;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Infrastructure.Repositories
{
    public class PackageHeaderRepository : IPackageHeaderRepository
    {

        private readonly EHealthDbContext _eHealthDbContext;

        public PackageHeaderRepository(EHealthDbContext eHealthDbContext)
        {
            _eHealthDbContext = eHealthDbContext;
        }

        public async Task<Guid> Create(PackageHeader input)
        {
            await _eHealthDbContext.PackageHeaders.AddAsync(input);
            await _eHealthDbContext.SaveChangesAsync();
            return input.Id;
        }

        public async Task<bool> Delete(PackageHeader input)
        {
            _eHealthDbContext.PackageHeaders.Update(input);
            return await _eHealthDbContext.SaveChangesAsync() > 0;
        }

        public async Task<PackageHeader?> Get(Guid id)
        {
            var res = await _eHealthDbContext.PackageHeaders.Where(x => x.Id == id)
                .Include(p => p.PackageType)
                .Include(p => p.PackageSubType)
                .Include(p => p.PackageComplexityClassification)
                .Include(p => p.GlobelPackageType)
                .Include(p => p.PackageSpecialty)
                .FirstOrDefaultAsync();
            if (res != null)
                return res;
            throw new DataNotFoundException();
        }

        public async Task<PagedResponse<PackageHeader>> Search(Expression<Func<PackageHeader, bool>> predicate, int pageNumber, int pageSize, bool enablePagination, string? orderBy, bool? ascending)
        {
            var query = _eHealthDbContext.PackageHeaders.Where(predicate)
               .Include(x => x.PackageSpecialty)
               .Include(x => x.GlobelPackageType)
               .Include(x => x.PackageComplexityClassification)
               .Include(x => x.PackageSubType).ThenInclude(t => t.PackageType).AsQueryable();
            if (!string.IsNullOrEmpty(orderBy))
            {
                switch (orderBy.Replace(" ","").ToLower())
                {
                    case "ehealthcode":
                        if (ascending == false)
                            query = query.OrderByDescending(e => e.EHealthCode);
                        else
                            query = query.OrderBy(e => e.EHealthCode);
                        break;
                    case "uhiacode":
                        if (ascending == false)
                            query = query.OrderByDescending(e => e.UHIACode);
                        else
                            query = query.OrderBy(e => e.UHIACode);
                        break;
                    case "packagenamear":
                        if (ascending == false)
                            query = query.OrderByDescending(e => e.NameAr);
                        else
                            query = query.OrderBy(e => e.NameAr);
                        break;
                    case "packagenameen":
                        if (ascending == false)
                            query = query.OrderByDescending(e => e.NameEn);
                        else
                            query = query.OrderBy(e => e.NameEn);
                        break;

                    case "packagetypear":
                        if (ascending == false)
                            query = query.OrderByDescending(e => e.PackageType.NameAr);
                        else
                            query = query.OrderBy(e => e.PackageType.NameAr);
                        break;
                    case "packagetypeen":
                        if (ascending == false)
                            query = query.OrderByDescending(e => e.PackageType.NameEN);
                        else
                            query = query.OrderBy(e => e.PackageType.NameEN);
                        break;
                    case "packagesubtypear":
                        if (ascending == false)
                            query = query.OrderByDescending(e => e.PackageSubType.NameAr);
                        else
                            query = query.OrderBy(e => e.PackageSubType.NameAr);
                        break;
                    case "packagesubtypeen":
                        if (ascending == false)
                            query = query.OrderByDescending(e => e.PackageSubType.NameEN);
                        else
                            query = query.OrderBy(e => e.PackageSubType.NameEN);
                        break;
                    case "packagecomplexityclassificationar":
                        if (ascending == false)
                            query = query.OrderByDescending(e => e.PackageComplexityClassification.DefinitionAr);
                        else
                            query = query.OrderBy(e => e.PackageComplexityClassification.DefinitionAr);
                        break;
                    case "packagecomplexityclassificationen":
                        if (ascending == false)
                            query = query.OrderByDescending(e => e.PackageComplexityClassification.DefinitionEn);
                        else
                            query = query.OrderBy(e => e.PackageComplexityClassification.DefinitionEn);
                        break;
                    case "globalpackagetypear":
                        if (ascending == false)
                            query = query.OrderByDescending(e => e.GlobelPackageType.DefinitionAr);
                        else
                            query = query.OrderBy(e => e.GlobelPackageType.DefinitionAr);
                        break;
                    case "globalpackagetypeen":
                        if (ascending == false)
                            query = query.OrderByDescending(e => e.GlobelPackageType.DefinitionEn);
                        else
                            query = query.OrderBy(e => e.GlobelPackageType.DefinitionEn);
                        break;
                    case "packagespecialityar":
                        if (ascending == false)
                            query = query.OrderByDescending(e => e.PackageSpecialty.DefinitionAr);
                        else
                            query = query.OrderBy(e => e.PackageSpecialty.DefinitionAr);
                        break;
                    case "packagespecialityen":
                        if (ascending == false)
                            query = query.OrderByDescending(e => e.PackageSpecialty.DefinitionEn);
                        else
                            query = query.OrderBy(e => e.PackageSpecialty.DefinitionEn);
                        break;
                    case "packageduration":
                        if (ascending == false)
                            query = query.OrderByDescending(e => e.PackageDuration);
                        else
                            query = query.OrderBy(e => e.PackageDuration);
                        break;
                    case "activationdatefrom":
                        if (ascending == false)
                            query = query.OrderByDescending(e => e.ActivationDateFrom);
                        else
                            query = query.OrderBy(e => e.ActivationDateFrom);
                        break;
                    case "activationdateto":
                        if (ascending == false)
                            query = query.OrderByDescending(e => e.ActivationDateTo);
                        else
                            query = query.OrderBy(e => e.ActivationDateTo);
                        break;
                    case "packageprice":
                        if (ascending == false)
                            query = query.OrderByDescending(e => e.PackagePrice);
                        else
                            query = query.OrderBy(e => e.PackagePrice);
                        break;
                    case "packageroundprice":
                        if (ascending == false)
                            query = query.OrderByDescending(e => e.PackageRoundPrice);
                        else
                            query = query.OrderBy(e => e.PackageRoundPrice);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                query = query.OrderByDescending(e=>e.ModifiedOn != null? e.ModifiedOn : e.CreatedOn);
            }
            return new PagedResponse<PackageHeader>
            {
                TotalCount =await query.CountAsync(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize :await query.CountAsync(),
                Data = enablePagination == true ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() :await query.ToListAsync()
            };
        }

        public async Task<bool> Update(PackageHeader input)
        {
            if (_eHealthDbContext.ChangeTracker.Entries<PackageHeader>().Any(a => a.State == EntityState.Modified))
            {
                return await _eHealthDbContext.SaveChangesAsync() > 0;
            }
            return false;
        }
    }
}
