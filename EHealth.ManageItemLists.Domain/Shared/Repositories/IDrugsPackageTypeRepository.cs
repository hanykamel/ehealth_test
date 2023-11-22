using EHealth.ManageItemLists.Domain.DrugsPackageTypes;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IDrugsPackageTypeRepository
    {
        Task<int> Create(DrugsPackageType input);
        Task<bool> Update(DrugsPackageType input);
        Task<bool> Delete(DrugsPackageType input);
        Task<DrugsPackageType?> Get(int id);
        Task<PagedResponse<DrugsPackageType>> Search(Expression<Func<DrugsPackageType,bool>> predicate, int pageNumber, int pageSize, bool enablePagination);
    }
}
