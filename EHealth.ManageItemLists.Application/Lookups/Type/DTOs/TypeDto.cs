using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.ItemListTypes;

namespace EHealth.ManageItemLists.Application.Lookups.Type.DTOs
{
    public class TypeDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string NameAr { get; set; }
        public string NameEN { get; set; }
        public bool IsDeleted { get; set; }


        public static TypeDto FromType(ItemListType itemListType) =>
        itemListType is not null?  new TypeDto
          {
              Id = itemListType.Id,
              Code = itemListType.Code,
              NameAr = itemListType.NameAr,
              NameEN = itemListType.NameEN,
              IsDeleted = itemListType.IsDeleted,
        } :null;
    }
}
