using EHealth.ManageItemLists.Domain.Packages.FeesOfResourcesPerUnitPackage.FeesOfResourcesPerUnitPackageComponents;
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
    public interface IFeesOfResourcesPerUnitPackageComponentRepository
    {
        Task<Guid> Create(FeesOfResourcesPerUnitPackageComponent input);
        Task<bool> Update(FeesOfResourcesPerUnitPackageComponent input);
        Task<bool> Delete(FeesOfResourcesPerUnitPackageComponent input);
        Task<FeesOfResourcesPerUnitPackageComponent?> Get(Guid id);
        Task<PagedResponse<FeesOfResourcesPerUnitPackageComponent>> Search(Expression<Func<FeesOfResourcesPerUnitPackageComponent, bool>> predicate, int pageNumber, int pageSize, bool enablePagination, string? orderBy, bool? ascending);
    }
}
