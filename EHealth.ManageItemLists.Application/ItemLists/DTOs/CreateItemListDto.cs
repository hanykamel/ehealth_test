using EHealth.ManageItemLists.Domain.ItemLists;

namespace EHealth.ManageItemLists.Application.ItemLists.DTOs
{
    public class CreateItemListDto
    {
        public string NameAr { get; set; }
        public string NameEN { get; set; }
        public int ItemListSubtypeId { get; set; }
        public int itemListTypeId { get; set; }
        public ItemList ToItemList(string code, string createdBy, string tenantId) => ItemList.Create(null, code, NameAr, NameEN, ItemListSubtypeId, createdBy, tenantId);
    }
}
