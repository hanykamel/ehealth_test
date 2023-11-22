using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.DrugsPackageTypes;
using EHealth.ManageItemLists.Domain.LocalTypeOfMeasure;
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
    public class DrugsPackageTypeRepository : IDrugsPackageTypeRepository
    {
        private readonly EHealthDbContext _eHealthDbContext;
        public DrugsPackageTypeRepository(EHealthDbContext eHealthDbContext)
        {
            _eHealthDbContext = eHealthDbContext;
        }
        public Task<int> Create(DrugsPackageType input)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(DrugsPackageType input)
        {
            throw new NotImplementedException();
        }

        public Task<DrugsPackageType?> Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResponse<DrugsPackageType>> Search(Expression<Func<DrugsPackageType, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            var query = _eHealthDbContext.DrugsPackageTypes.Where(predicate).AsQueryable();

            query = query.OrderBy(x => x.NameEN);
            return new PagedResponse<DrugsPackageType>
            {
                TotalCount = await query.CountAsync(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize : await query.CountAsync(),
                Data = enablePagination == true ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() : await query.ToListAsync()
            };
        }

        public Task<bool> Update(DrugsPackageType input)
        {
            throw new NotImplementedException();
        }
    }
}
