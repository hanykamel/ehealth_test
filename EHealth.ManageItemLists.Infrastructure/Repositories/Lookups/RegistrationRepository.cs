using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.LocalTypeOfMeasure;
using EHealth.ManageItemLists.Domain.RegistrationTypes;
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
    public class RegistrationRepository : IRegistrationRepository
    {
        private readonly EHealthDbContext _eHealthDbContext;
        public RegistrationRepository(EHealthDbContext eHealthDbContext)
        {
            _eHealthDbContext = eHealthDbContext;    
        }
        public Task<int> CreateRegistrationType(RegistrationType input)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteRegistrationType(RegistrationType input)
        {
            throw new NotImplementedException();
        }

        public Task<RegistrationType?> Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResponse<RegistrationType>> Search(Expression<Func<RegistrationType, bool>> predicate, int pageNumber, int pageSize, bool enablePagination)
        {
            var query = _eHealthDbContext.RegistrationTypes.Where(predicate).AsQueryable();

            query = query.OrderBy(x => x.RegistrationTypeENG);
            return new PagedResponse<RegistrationType>
            {
                TotalCount = await query.CountAsync(),
                PageNumber = pageNumber,
                PageSize = enablePagination == true ? pageSize : await query.CountAsync(),
                Data = enablePagination == true ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() : await query.ToListAsync()
            };
        }


        public Task<bool> UpdateRegistrationType(RegistrationType input)
        {
            throw new NotImplementedException();
        }
    }
}
