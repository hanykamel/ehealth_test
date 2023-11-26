using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.DTOs;
using EHealth.ManageItemLists.Application.DoctorFees.ItemPrice.DTOs;
using EHealth.ManageItemLists.Application.ItemListPrices.DTOs;
using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.LocalUnitOfMeasures.DTOs;
using EHealth.ManageItemLists.Application.Lookups.SubCategories.DTOs;
using EHealth.ManageItemLists.Domain.ConsumablesAndDevices;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.PackageItems.DTOs
{
    public class GetAllConsumablesAndDevicesDTO
    {
        public string? eHealthCode { get; set; }
        public string? UHIAId { get; set; }
        public string? ItemNameEn { get; set; }
        public string? ItemNameAr { get; set; }
        public CategoryDto? serviceCategory { get; set; }
        public SubCategoryDto? subCategory { get; set; }
        public UnitOfMeasureDto unitOfMeasure { get; set; }
        public ItemListPriceDto Price { get; set; }

        public static GetAllConsumablesAndDevicesDTO FromGetAllConsumablesAndDevicesDTO(ConsumablesAndDevicesUHIA input) =>
            new GetAllConsumablesAndDevicesDTO
            {
                eHealthCode = input.EHealthCode,
                UHIAId = input.UHIAId,
                ItemNameAr= input.ShortDescriptorAr,
                ItemNameEn=input.ShortDescriptorEn,
                Price = ItemListPriceDto.FromItemListPrice(input.ItemListPrices.OrderByDescending(o => o.EffectiveDateFrom).FirstOrDefault()),
                serviceCategory = CategoryDto.FromCategory(input.ServiceCategory),
                subCategory = SubCategoryDto.FromSubCategory(input.SubCategory),
                unitOfMeasure = UnitOfMeasureDto.FromLocalUnitOfMeasure(input.UnitOfMeasure)
            };

        

    }
}
