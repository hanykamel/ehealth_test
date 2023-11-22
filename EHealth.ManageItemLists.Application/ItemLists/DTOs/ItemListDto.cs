using EHealth.ManageItemLists.Application.Lookups.Subtype.DTOs;
using EHealth.ManageItemLists.Application.Lookups.Type.DTOs;
using EHealth.ManageItemLists.Domain.ItemLists;
using System.ComponentModel.DataAnnotations;

namespace EHealth.ManageItemLists.Application.ItemLists.DTOs
{
    public class ItemListDto
    {
        public int Id { get; private set; }
        public string NameAr { get; private set; }
        public string NameEN { get; private set; }
        public TypeDto itemListType { get; set; }
        public SubTypeDto itemListSubtype { get; private set; }

        public int ItemListTypeId { get; set; }
        public int ItemListSubtypeId { get; private set; }
        public string Code { get; set; }
        public string? UpdatedBy { get; set; }

        public string? UpdatedOn { get; set; }
        public int ItemCounts { get; set; }
        public int MyProperty { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }

        public bool IsBusy { get; set; }

        public static ItemListDto FromItemList(ItemList itemList) =>
            new ItemListDto
            {
                Id = itemList.Id,
                Code = itemList.Code,
                NameAr = itemList.NameAr,
                NameEN = itemList.NameEN,
                ItemListTypeId = itemList.ItemListSubtype.ItemListTypeId,
                ItemListSubtypeId = itemList.ItemListSubtypeId,
                itemListType = TypeDto.FromType(itemList.ItemListSubtype.ItemListType),
                itemListSubtype = SubTypeDto.FromSubtype(itemList.ItemListSubtype),
                IsDeleted = itemList.IsDeleted,
                Active = itemList.Active,
                IsBusy = itemList.IsBusy,
                UpdatedBy = itemList.ModifiedBy,
                UpdatedOn = itemList.ModifiedOn?.ToString("yyyy-MM-dd hh:mm"),
                ItemCounts = itemList.serviceUHIAlist.Count() > 0 ? itemList.serviceUHIAlist.Count()
                           : itemList.ConsumablesAndDevicesUHIAlist.Count() > 0 ? itemList.ConsumablesAndDevicesUHIAlist.Count()
                           : itemList.ProcedureICHIlist.Count() > 0 ? itemList.ProcedureICHIlist.Count()
                           : itemList.DevicesAndAssetsUHIAlist.Count() > 0 ? itemList.DevicesAndAssetsUHIAlist.Count()
                           : itemList.FacilityUHIAlist.Count() > 0 ? itemList.FacilityUHIAlist.Count()
                           : itemList.ResourceUHIAlist.Count() > 0 ? itemList.ResourceUHIAlist.Count()
                           : itemList.DrugUHIAlist.Count() > 0 ? itemList.DrugUHIAlist.Count()
                           : itemList.DoctorFeesUHIAlist.Count() > 0 ? itemList.DoctorFeesUHIAlist.Count() : 0,

            };
    }
}




