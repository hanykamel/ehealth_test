using EHealth.ManageItemLists.Domain.ItemListSubtypes;
using EHealth.ManageItemLists.Domain.ItemListTypes;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Sub_Categories;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IItemListSubtypeRepository
    {
        Task<PagedResponse<ItemListSubtype>> Search(Expression<Func<ItemListSubtype, bool>> predicate, int pageNumber, int pageSize, bool enablePagination);
        Task<ItemListSubtype> GetById(int id);
    }
}
