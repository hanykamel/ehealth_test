using EHealth.ManageItemLists.Domain.Packages.FeesOfResourcesPerUnitPackage.FeesOfResourcesPerUnitPackageSummaries;
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackageComponents;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IFeesOfResourcesPerUnitPackageSummaryRepository
    {
        Task<Guid> Create(FeesOfResourcesPerUnitPackageSummary input);
        Task<bool> Update(FeesOfResourcesPerUnitPackageSummary input);
        Task<bool> Delete(FeesOfResourcesPerUnitPackageSummary input);
        Task<FeesOfResourcesPerUnitPackageSummary?> Get(Guid id);
        Task<PagedResponse<FeesOfResourcesPerUnitPackageSummary>> Search(Expression<Func<FeesOfResourcesPerUnitPackageSummary, bool>> predicate, int pageNumber, int pageSize, bool enablePagination, string? orderBy, bool? ascending);
    }
}
