using EHealth.ManageItemLists.Domain.ItemListTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.ItemLists.DTOs
{
    public class ItemListTypesDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public bool Active { get; set; }
        public string NameAr { get; set; }
        public string NameEN { get; set; }

        public static ItemListTypesDTO FromItemListType(ItemListType itemListType) =>
            new ItemListTypesDTO
            {
                Id = itemListType.Id,
                NameAr = itemListType.NameAr,
                NameEN = itemListType.NameEN,
                Code = itemListType.Code,
                Active = itemListType.Active
            };
    }
}
