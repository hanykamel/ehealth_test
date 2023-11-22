using EHealth.ManageItemLists.Domain.ItemListPricing;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using EHealth.ManageItemLists.Domain.Sub_Categories;
using System.Linq.Expressions;

namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IItemListPriceRepository
    {
        Task<int> Create(ItemListPrice input);
        Task<bool> CreateRange(List<ItemListPrice> input);       
        Task<bool> Update(ItemListPrice input);
        Task<bool> Delete(ItemListPrice input);
        Task<ItemListPrice?> Get(int id);
        Task<PagedResponse<ItemListPrice>> Search(Expression<Func<ItemListPrice, bool>> predicate, int pageNumber, int pageSize, bool enablePagination);
    }
}
