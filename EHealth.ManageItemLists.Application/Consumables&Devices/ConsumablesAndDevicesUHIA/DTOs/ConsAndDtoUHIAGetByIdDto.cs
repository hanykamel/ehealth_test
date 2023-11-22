using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.ItemListPrices.DTOs;
using EHealth.ManageItemLists.Application.Lookups.LocalUnitOfMeasures.DTOs;
using EHealth.ManageItemLists.Application.Lookups.SubCategories.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.DTOs
{
    public class ConsAndDtoUHIAGetByIdDto
    {
        public Guid Id { get; set; }
        public string EHealthCode { get; set; }
        public string UHIAId { get; set; }
        public string? ShortDescAr { get; set; }
        public string? ShortDescEn { get; set; }
        public CategoryDto ServiceCategory { get; set; }
        public SubCategoryDto ServiceSubCategory { get; set; }  // discus for whole object or only the required
        public UnitOfMeasureDto UnitOfMeasure { get; set; }
        public int ServiceCategoryId { get; set; }
        public int ServiceSubCategoryId { get; set; }
        public int UnitOfMeasureId { get; set; }
        public int ItemListId { get; set; }
        public IList<ItemListPriceDto> ItemListPrices { get; set; } = new List<ItemListPriceDto>();
        public string DataEffectiveDateFrom { get; set; }
        public string? DataEffectiveDateTo { get; set; }
        public bool IsDeleted { get; set; }


        public static ConsAndDtoUHIAGetByIdDto FromConsAndDevsUHIAGetById(Domain.ConsumablesAndDevices.ConsumablesAndDevicesUHIA input) =>
       new ConsAndDtoUHIAGetByIdDto
       {
           Id = input.Id,
           EHealthCode = input.EHealthCode,
           ShortDescAr = input.ShortDescriptorAr,
           ShortDescEn = input.ShortDescriptorEn,
           UHIAId=input.UHIAId,
           ServiceCategoryId = input.ServiceCategoryId,
           ServiceSubCategoryId = input.SubCategoryId,
           UnitOfMeasureId = input.UnitOfMeasureId,
           UnitOfMeasure = UnitOfMeasureDto.FromLocalUnitOfMeasure(input.UnitOfMeasure),
           ServiceCategory = CategoryDto.FromCategory(input.ServiceCategory),
           ServiceSubCategory = SubCategoryDto.FromSubCategory(input.SubCategory),
           DataEffectiveDateFrom = input.DataEffectiveDateFrom.ToString("yyyy-MM-dd"),
           DataEffectiveDateTo = input.DataEffectiveDateTo?.ToString("yyyy-MM-dd"),      
           ItemListPrices= ItemListPriceDto.FromItemPrice(input.ItemListPrices),
           ItemListId = input.ItemListId,
           IsDeleted = input.IsDeleted
       };
    }
}
