using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.PackageSpecialties;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Infrastructure.Repositories.Lookups
{
    public class PackageSpecialtiesRepository : IPackageSpecialtiesRepository
    {
        private readonly EHealthDbContext _eHealthDbContext;
        public PackageSpecialtiesRepository(EHealthDbContext eHealthDbContext)
        {
            _eHealthDbContext = eHealthDbContext;
        }
        public Task<int> Create(PackageSpecialty input)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(PackageSpecialty input)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResponse<PackageSpecialty>> Search(Expression<Func<PackageSpecialty, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            var query = _eHealthDbContext.PackageSpecialties.Where(predicate)
                           .AsQueryable();

            query = query.OrderBy(x => x.SpecialtyEn);
            return new PagedResponse<PackageSpecialty>
            {
                TotalCount = await query.CountAsync(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize : await query.CountAsync(),
                Data = enablePagination == true ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() : await query.ToListAsync()
            };
        }
        public Task<bool> Update(PackageSpecialty input)
        {
            throw new NotImplementedException();
        }

        Task<PackageSpecialty?> IPackageSpecialtiesRepository.Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
