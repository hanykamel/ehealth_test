using EHealth.ManageItemLists.Domain.Packages.FeesOfResourcesPerUnitPackage.FeesOfResourcesPerUnitPackageSummaries;
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackageSummaries;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IInvestmentCostPackageSummaryRepository
    {
        Task<Guid> Create(InvestmentCostPackageSummary input);
        Task<bool> Update(InvestmentCostPackageSummary input);
        Task<bool> Delete(InvestmentCostPackageSummary input);
        Task<InvestmentCostPackageSummary?> Get(Guid id);
        Task<PagedResponse<InvestmentCostPackageSummary>> Search(Expression<Func<InvestmentCostPackageSummary, bool>> predicate, int pageNumber, int pageSize, bool enablePagination, string? orderBy, bool? ascending);

    }
}
