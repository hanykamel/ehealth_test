using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.Type.DTOs;
using EHealth.ManageItemLists.Domain.ItemListSubtypes;
using EHealth.ManageItemLists.Domain.ItemListTypes;

namespace EHealth.ManageItemLists.Application.Lookups.Subtype.DTOs
{
    public class SubTypeDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string NameAr { get; set; }
        public string NameEN { get; set; }
        public bool IsDeleted { get; set; }

        public TypeDto ItemListType { get; set; }
        public static SubTypeDto FromSubtype(ItemListSubtype itemListSubtype)
         => itemListSubtype is not null ? new SubTypeDto
         {
             Id = itemListSubtype.Id,
             Code = itemListSubtype.Code,
             NameAr = itemListSubtype.NameAr,
             NameEN = itemListSubtype.NameEN,
             ItemListType = TypeDto.FromType(itemListSubtype.ItemListType),
             IsDeleted = itemListSubtype.IsDeleted
         } : null;
 
    }
}
