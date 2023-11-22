using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackageComponents;
using EHealth.ManageItemLists.Domain.Packages.PackageHeaders;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IInvestmentCostPackageComponentRepository
    {
        Task<Guid> Create(InvestmentCostPackageComponent input);
        Task<bool> Update(InvestmentCostPackageComponent input);
        Task<bool> Delete(InvestmentCostPackageComponent input);
        Task<InvestmentCostPackageComponent?> Get(Guid id);
        Task<PagedResponse<InvestmentCostPackageComponent>> Search(Expression<Func<InvestmentCostPackageComponent, bool>> predicate, int pageNumber, int pageSize, bool enablePagination, string? orderBy, bool? ascending);
    }
}
