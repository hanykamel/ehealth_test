using EHealth.ManageItemLists.Domain.ItemListTypes;
using EHealth.ManageItemLists.Domain.ItemTypes;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IItemListTypeRepository
    {
        Task<PagedResponse<ItemListType>> Search(Expression<Func<ItemListType, bool>> predicate, int pageNumber, int pageSize, bool enablePagination);
        Task<ItemListType> GetById(int id);
    }
}
