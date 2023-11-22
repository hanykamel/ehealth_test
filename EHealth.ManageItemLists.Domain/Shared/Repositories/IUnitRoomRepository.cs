using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.UnitRooms;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IUnitRoomRepository
    {
        Task<int> CreateUnitRoom(UnitRoom input);
        Task<bool> UpdateUnitRoom(UnitRoom input);
        Task<bool> DeleteUnitRoom(UnitRoom input);
        Task<UnitRoom?> Get(int id);
        Task<PagedResponse<UnitRoom>> Search(IUnitRoomRepository repository, Expression<Func<UnitRoom, bool>> predicate, int pageNumber, int pageSize, bool enablePagination);
    }
}
