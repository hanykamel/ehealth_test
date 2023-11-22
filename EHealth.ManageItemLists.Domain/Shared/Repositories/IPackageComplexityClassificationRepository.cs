using EHealth.ManageItemLists.Domain.PackageComplexityClassifications;
using EHealth.ManageItemLists.Domain.Prescribinglevels;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IPackageComplexityClassificationRepository
    {
        Task<int> Create(PackageComplexityClassification input);
        Task<bool> Update(PackageComplexityClassification input);
        Task<bool> Delete(PackageComplexityClassification input);
        Task<PackageComplexityClassification?> Get(int id);
        Task<PagedResponse<PackageComplexityClassification>> Search(Expression<Func<PackageComplexityClassification, bool>> predicate, int pageNumber, int pageSize, bool enablePagination);
    }
}
