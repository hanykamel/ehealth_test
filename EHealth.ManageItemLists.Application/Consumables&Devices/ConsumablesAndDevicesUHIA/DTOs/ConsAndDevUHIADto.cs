using EHealth.ManageItemLists.Application.ItemListPrices.DTOs;
using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.LocalUnitOfMeasures.DTOs;
using EHealth.ManageItemLists.Application.Lookups.SubCategories.DTOs;

namespace EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.DTOs
{
    public class ConsAndDevDto
    {
        public Guid Id { get; set; }
        public int ItemListId { get; set; }
        public string EHealthCode { get; set; }
        public string UHIAId { get; set; }
        public string? ShortDescriptionAr { get; set; }
        public string? ShortDescriptionEn { get; set; }   
        public CategoryDto Category { get; set; }
        public SubCategoryDto SubCategory { get; set; }  
        public UnitOfMeasureDto LocalUnitOfMeasure { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int LocalUnitOfMeasureId { get; set; }
        public string DataEffectiveDateFrom { get;  set; }
    
        public string? DataEffectiveDateTo { get;  set; }
        public ItemListPriceDto ItemListPrice { get; set; }
        public bool IsDeleted { get; set; }

        public static ConsAndDevDto FromConsAndDevsUHIA(Domain.ConsumablesAndDevices.ConsumablesAndDevicesUHIA input) =>
        new ConsAndDevDto
        {
            Id = input.Id,
            ItemListId = input.ItemListId,
            EHealthCode = input.EHealthCode,
            UHIAId = input.UHIAId,
            ShortDescriptionAr = input.ShortDescriptorAr,
            ShortDescriptionEn = input.ShortDescriptorEn,
            CategoryId=input.ServiceCategoryId, 
            SubCategoryId=input.SubCategoryId,
            LocalUnitOfMeasureId=input.UnitOfMeasureId,
            LocalUnitOfMeasure= UnitOfMeasureDto.FromLocalUnitOfMeasure(input.UnitOfMeasure),
            Category = CategoryDto.FromCategory(input.ServiceCategory),
            SubCategory = SubCategoryDto.FromSubCategory(input.SubCategory),
            DataEffectiveDateFrom= input.DataEffectiveDateFrom.ToString("yyyy-MM-dd"),
            DataEffectiveDateTo=input.DataEffectiveDateTo?.ToString("yyyy-MM-dd"),
            ItemListPrice = ItemListPriceDto.FromItemListPrice(input.ItemListPrices.OrderByDescending(e => e.EffectiveDateFrom).FirstOrDefault()),
            IsDeleted = input.IsDeleted
        };
    }
}
