using EHealth.ManageItemLists.DataAccess;
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackageSummaries;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Infrastructure.Repositories
{
    public class InvestmentCostPackageSummaryRepository : IInvestmentCostPackageSummaryRepository
    {

        private readonly EHealthDbContext _eHealthDbContext;

        public InvestmentCostPackageSummaryRepository(EHealthDbContext eHealthDbContext)
        {
            _eHealthDbContext = eHealthDbContext;
        }


        public async Task<Guid> Create(InvestmentCostPackageSummary input)
        {
            await _eHealthDbContext.InvestmentCostPackageSummaries.AddAsync(input);
            await _eHealthDbContext.SaveChangesAsync();
            return input.Id;
        }

        public Task<bool> Delete(InvestmentCostPackageSummary input)
        {
            throw new NotImplementedException();
        }

        public Task<InvestmentCostPackageSummary?> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResponse<InvestmentCostPackageSummary>> Search(Expression<Func<InvestmentCostPackageSummary, bool>> predicate, int pageNumber, int pageSize, bool enablePagination, string? orderBy, bool? ascending)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(InvestmentCostPackageSummary input)
        {
            throw new NotImplementedException();
        }
    }
}
