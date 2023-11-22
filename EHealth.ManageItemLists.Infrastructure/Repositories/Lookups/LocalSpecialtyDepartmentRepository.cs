using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.LocalSpecialtyDepartments;
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
    public class LocalSpecialtyDepartmentRepository : ILocalSpecialtyDepartmentsRepository
    {
        private readonly EHealthDbContext _eHealthDbContext;
        public LocalSpecialtyDepartmentRepository(EHealthDbContext eHealthDbContext)
        {
            _eHealthDbContext = eHealthDbContext;
        }

        public Task<int> CreateLocalSpecialtyDepartments(LocalSpecialtyDepartment input)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteLocalSpecialtyDepartments(LocalSpecialtyDepartment input)
        {
            throw new NotImplementedException();
        }

        public Task<LocalSpecialtyDepartment?> Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResponse<LocalSpecialtyDepartment>> Search(Expression<Func<LocalSpecialtyDepartment, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            var query = _eHealthDbContext.LocalSpecialtyDepartments.Where(predicate).AsQueryable();

            query = query.OrderBy(x => x.LocalSpecialityENG);
            return new PagedResponse<LocalSpecialtyDepartment>
            {
                TotalCount = await query.CountAsync(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize : await query.CountAsync(),
                Data = enablePagination == true ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() : await query.ToListAsync()
            };
        }

        public Task<bool> UpdateLocalSpecialtyDepartments(LocalSpecialtyDepartment input)
        {
            throw new NotImplementedException();
        }
    }
}
