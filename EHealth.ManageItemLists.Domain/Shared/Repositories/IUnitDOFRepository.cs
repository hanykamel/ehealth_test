using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.UnitOfTheDoctor_sfees;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IUnitDOFRepository
    {
        Task<int> CreateUnitDOF(UnitDOF input);
        Task<bool> UpdateUnitDOF(UnitDOF input);
        Task<bool> DeleteUnitDOF(UnitDOF input);
        Task<UnitDOF?> Get(int id);
        Task<PagedResponse<UnitDOF>> Search(Expression<Func<UnitDOF, bool>> predicate, int pageNumber, int pageSize, bool enablePagination);
    }
}
