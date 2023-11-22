using EHealth.ManageItemLists.Application.Drugs.UHIA.DTOs;
using EHealth.ManageItemLists.Application.Facility.UHIA.DTOs;
using EHealth.ManageItemLists.Application.ItemListPrices.DTOs;
using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.SubCategories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.UnitRooms.DTOs;
using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using EHealth.ManageItemLists.Domain.Facility.UHIA;
using EHealth.ManageItemLists.Domain.ItemListPricing;
using EHealth.ManageItemLists.Domain.Sub_Categories;
using EHealth.ManageItemLists.Domain.UnitRooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.DTOs
{
    public class DevicesAndAssetsUHIADto
    {
        public Guid Id { get; set; }
        public string EHealthCode { get; set; }
        public string DescriptorAr { get; set; }
        public string DescriptorEn { get; set; }
        public UnitRoomDto? UnitRoom { get; set; }
        public SubCategoryDto SubCategory { get; set; }
        public CategoryDto Category { get; set; }
        public int? UnitRoomId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int ItemListId { get; set; }
        //public DateTime DataEffectiveDateFrom { get; set; }
        //public DateTime? DataEffectiveDateTo { get; set; }  
        public string DataEffectiveDateFrom { get; set; }
        public string? DataEffectiveDateTo { get; set; }
        public ItemListPriceDto ItemListPrice { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }

        public static DevicesAndAssetsUHIADto FromDevicesAndAssetsUHIA(DevicesAndAssetsUHIA input) =>
        new DevicesAndAssetsUHIADto
        {
            Id = input.Id,
            EHealthCode = input.Code,
            DescriptorAr = input.DescriptorAr,
            DescriptorEn = input.DescriptorEn,
            UnitRoom = UnitRoomDto.FromUnitRoom(input.UnitRoom),
            Category = CategoryDto.FromCategory(input.Category),
            SubCategory = SubCategoryDto.FromSubCategory(input.SubCategory),
            DataEffectiveDateFrom = input.DataEffectiveDateFrom.ToString("yyyy-MM-dd"),
            DataEffectiveDateTo = input.DataEffectiveDateTo?.ToString("yyyy-MM-dd"),
            ItemListPrice = ItemListPriceDto.FromItemListPrice(input.ItemListPrices.OrderByDescending(e => e.EffectiveDateFrom).FirstOrDefault()),
            ModifiedBy = input.ModifiedBy,
            ModifiedOn = input.ModifiedOn?.ToString("yyyy-MM-dd"),
            ItemListId = input.ItemListId,
            CategoryId = input.CategoryId,
            SubCategoryId = input.SubCategoryId,
            UnitRoomId = input.UnitRoomId,
            IsDeleted = input.IsDeleted
        };
    }
}

