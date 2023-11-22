using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostDepreciationsAndMaintenances;
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackagAssets;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IInvestmentCostDepreciationAndMaintenanceRepository
    {
        Task<Guid> Create(InvestmentCostDepreciationAndMaintenance input);
        Task<bool> Update(InvestmentCostDepreciationAndMaintenance input);
        Task<bool> Delete(InvestmentCostDepreciationAndMaintenance input);
        Task<InvestmentCostDepreciationAndMaintenance?> Get(Guid id);
        Task<PagedResponse<InvestmentCostDepreciationAndMaintenance>> Search(Expression<Func<InvestmentCostDepreciationAndMaintenance, bool>> predicate, int pageNumber, int pageSize, bool enablePagination, string? orderBy, bool? ascending);
    }
}
