using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.ItemLists;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IItemListRepository
    {
        Task<int> Create(ItemList input);
        Task<bool> Update(ItemList input);
        Task<bool> Delete(ItemList input);
        Task<ItemList?> Get(int id);
        Task<PagedResponse<ItemList>> Search(Expression<Func<ItemList, bool>> predicate, int pageNumber, int pageSize, string? orderBy, bool? ascending, bool enablePagination);
        Task<bool> IsListBusy(int Id);
    }
}
