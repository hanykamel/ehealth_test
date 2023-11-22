using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.UnitsTypes;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IUnitsTypeRepository
    {
        Task<int> Create(UnitsType input);
        Task<bool> Update(UnitsType input);
        Task<bool> Delete(UnitsType input);
        Task<UnitsType?> Get(int id);
        Task<PagedResponse<UnitsType>> Search(Expression<Func<UnitsType, bool>> predicate, int pageNumber, int pageSize, bool enablePagination);
    }
}
