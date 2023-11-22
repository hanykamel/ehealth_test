using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.PackageComplexityClassifications;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Infrastructure.Repositories.Lookups
{
    public class PackageComplexityClassificationRepository : IPackageComplexityClassificationRepository
    {
        private readonly EHealthDbContext _eHealthDbContext;
        public PackageComplexityClassificationRepository(EHealthDbContext eHealthDbContext)
        {
            _eHealthDbContext = eHealthDbContext;
        }
        public Task<int> Create(PackageComplexityClassification input)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(PackageComplexityClassification input)
        {
            throw new NotImplementedException();
        }

        public Task<PackageComplexityClassification?> Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResponse<PackageComplexityClassification>> Search(Expression<Func<PackageComplexityClassification, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            var query = _eHealthDbContext.PackageComplexityClassifications.Where(predicate)
                .AsQueryable();

            query = query.OrderBy(x => x.ComplexityEn);
            return new PagedResponse<PackageComplexityClassification>
            {
                TotalCount = await query.CountAsync(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize : await query.CountAsync(),
                Data = enablePagination == true ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() : await query.ToListAsync()
            };
        }

        public Task<bool> Update(PackageComplexityClassification input)
        {
            throw new NotImplementedException();
        }
    }
}
