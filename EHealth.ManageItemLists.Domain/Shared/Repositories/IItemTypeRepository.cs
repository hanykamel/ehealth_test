using EHealth.ManageItemLists.Domain.ItemTypes;
using EHealth.ManageItemLists.Domain.Shared.Pagination;


namespace EHealth.ManageItemLists.Domain.Shared.Repositories
{
    public interface IItemTypeRepository
    {
        Task<int> CreateItemType(ItemType input);
        Task<bool> UpdateItemType(ItemType input);
        Task<bool> DeleteItemType(ItemType input);
        Task<ItemType?> Get(int id);
        Task<PagedResponse<ItemType>> Search(int? id, string? code, string? nameAr, string? nameEN, string? DefinitionAr, string? DefinitionEN, bool Active, int pageNumber, int pageSize);
    }
}
