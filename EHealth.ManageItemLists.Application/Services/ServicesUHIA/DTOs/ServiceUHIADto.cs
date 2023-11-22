using EHealth.ManageItemLists.Application.ItemListPrices.DTOs;
using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.SubCategories.DTOs;
using EHealth.ManageItemLists.Domain.ItemListPricing;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using System.Globalization;

namespace EHealth.ManageItemLists.Application.Services.ServicesUHIA.DTOs
{
    public class ServiceUHIADto
    {
        public Guid Id { get; set; }
        public int ItemListId { get; set; }
        public string EHealthCode { get; set; }
        public string UHIAId { get; set; }
        public string? ShortDescAr { get; set; }
        public string? ShortDescEn { get; set; }
        public CategoryDto ServiceCategory { get; set; }
        public SubCategoryDto ServiceSubCategory { get; set; }
        public string DataEffectiveDateFrom { get; set; }
        public string? DataEffectiveDateTo { get; set; }
        public ItemListPriceDto ItemListPrice { get; set; }
        public bool IsDeleted { get; set; }
        public static ServiceUHIADto FromServiceUHIA(ServiceUHIA input) =>
        new ServiceUHIADto
        {
            Id = input.Id,
            ItemListId = input.ItemListId,
            EHealthCode = input.EHealthCode,
            ShortDescAr = input.ShortDescAr,
            ShortDescEn = input.ShortDescEn,
            UHIAId=input.UHIAId,
            ServiceCategory = CategoryDto.FromCategory(input.ServiceCategory),
            ServiceSubCategory = SubCategoryDto.FromSubCategory(input.ServiceSubCategory),
            DataEffectiveDateFrom = input.DataEffectiveDateFrom.ToString("yyyy-MM-dd"),
            DataEffectiveDateTo = input.DataEffectiveDateTo?.ToString("yyyy-MM-dd"),
            ItemListPrice = ItemListPriceDto.FromItemListPrice(input.ItemListPrices.OrderByDescending(e => e.EffectiveDateFrom).FirstOrDefault()),
            IsDeleted = input.IsDeleted,
        };
   
    }
}
