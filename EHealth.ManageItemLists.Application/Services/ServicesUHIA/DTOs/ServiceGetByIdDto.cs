using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.ItemListPrices.DTOs;
using EHealth.ManageItemLists.Application.Lookups.SubCategories.DTOs;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using MediatR;
using System.Globalization;

namespace EHealth.ManageItemLists.Application.Services.ServicesUHIA.DTOs
{
    public class ServiceGetByIdDto
    {
        public Guid Id { get; set; }
        public string EHealthCode { get; set; }
        public string UHIAId { get; set; }
        public string? ShortDescAr { get; set; }
        public string? ShortDescEn { get; set; }
        public CategoryDto ServiceCategory { get; set; }
        public SubCategoryDto ServiceSubCategory { get; set; }
        public int ServiceCategoryId { get; set; }
        public int ServiceSubCategoryId { get; set; }
        public int ItemListId { get; set; }
        public string DataEffectiveDateFrom { get; set; }
        public string? DataEffectiveDateTo { get; set; }
        public IList<ItemListPriceDto> ItemListPrices { get; set; } = new List<ItemListPriceDto>();
        public bool IsDeleted { get; set; }

        public static ServiceGetByIdDto FromServiceUHIA(ServiceUHIA input) =>
       new ServiceGetByIdDto
       {
           Id = input.Id,
           UHIAId =input.UHIAId,
           EHealthCode = input.EHealthCode,
           ShortDescAr = input.ShortDescAr,
           ShortDescEn = input.ShortDescEn,
           ServiceCategoryId=input.ServiceCategoryId,
           ServiceSubCategoryId=input.ServiceSubCategoryId,
           ItemListId=input.ItemListId,
           ServiceCategory = CategoryDto.FromCategory(input.ServiceCategory),
           ServiceSubCategory = SubCategoryDto.FromSubCategory(input.ServiceSubCategory),
           DataEffectiveDateFrom = input.DataEffectiveDateFrom.ToString("yyyy-MM-dd"),
           DataEffectiveDateTo = input.DataEffectiveDateTo?.ToString("yyyy-MM-dd"),
           ItemListPrices = ItemListPriceDto.FromItemPrice(input.ItemListPrices),
           IsDeleted = input.IsDeleted,
       };
    }
}
