using EHealth.ManageItemLists.Domain.ItemLists;

namespace EHealth.ManageItemLists.Application.ItemLists.DTOs
{
    public class UpdateItemListDto
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEN { get; set; }
        public int ItemListSubtypeId { get; set; }
        public bool Active { get; set; }
        public ItemList ToItemList(string code, string createdBy, string tenantId) => ItemList.Create(Id, code, NameAr, NameEN, ItemListSubtypeId, createdBy, tenantId);
    }
}
