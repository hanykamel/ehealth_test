using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.UnitOfTheDoctor_sfees;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Infrastructure.Repositories.Lookups
{
    public class UnitDOFRepository : IUnitDOFRepository
    {
        private readonly EHealthDbContext _eHealthDbContext;
        public UnitDOFRepository(EHealthDbContext eHealthDbContext)
        {
            _eHealthDbContext = eHealthDbContext;
        }
        public Task<int> CreateUnitDOF(UnitDOF input)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUnitDOF(UnitDOF input)
        {
            throw new NotImplementedException();
        }

        public Task<UnitDOF?> Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResponse<UnitDOF>> Search(Expression<Func<UnitDOF, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            var query = _eHealthDbContext.UnitsOfTheDoctorFees.Where(predicate)
                .AsQueryable();

            query = query.OrderBy(x => x.NameEN);
            return new PagedResponse<UnitDOF>
            {
                TotalCount = await query.CountAsync(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize : await query.CountAsync(),
                Data = enablePagination == true ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() : await query.ToListAsync()
            };
        }

        public Task<bool> UpdateUnitDOF(UnitDOF input)
        {
            throw new NotImplementedException();
        }
    }
}
