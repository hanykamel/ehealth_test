using EHealth.ManageItemLists.Domain.PriceUnits;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IPriceUnitRepository
    {
        Task<int> CreatePriceUnit(PriceUnit input);
        Task<bool> UpdatePriceUnit(PriceUnit input);
        Task<bool> DeletePriceUnit(PriceUnit input);
        Task<PriceUnit?> Get(int id);
        Task<PagedResponse<PriceUnit>> Search(Expression<Func<PriceUnit, bool>> predicate, int pageNumber, int pageSize, bool enablePagination);
    }
}
