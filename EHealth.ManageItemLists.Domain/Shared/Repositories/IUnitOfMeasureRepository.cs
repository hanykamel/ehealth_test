using EHealth.ManageItemLists.Domain.LocalTypeOfMeasure;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IUnitOfMeasureRepository
    {
        Task<int> CreateUnitOfMeasure(UnitOfMeasure input);
        Task<bool> UpdateUnitOfMeasure(UnitOfMeasure input);
        Task<bool> DeleteUnitOfMeasure(UnitOfMeasure input);
        Task<UnitOfMeasure?> Get(int id);
        Task<PagedResponse<UnitOfMeasure>> Search(Expression<Func<UnitOfMeasure, bool>> predicate, int pageNumber, int pageSize, bool enablePagination);
    }
}
