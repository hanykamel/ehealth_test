using EHealth.ManageItemLists.Application.Lookups.PriceUnits.DTOs;
using EHealth.ManageItemLists.Application.Resource.ItemPrice.DTOs;
using EHealth.ManageItemLists.Domain.PriceUnits;
using EHealth.ManageItemLists.Domain.Resource.UHIA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.PackageItems.DTOs
{
    public class GetAllResourceItemsDTO
    {
        public Guid Id { get; set; }
        public string? ItemAr { get; set; }
        public string ItemEn { get; set; }
        public string? Code { get; set; }
        public string? CategoryAr { get; set; }
        public string? CategoryEn { get; set; }
        public string? SubCategoryAr { get; set; }
        public string? SubCategoryEn { get; set; }
        public double? Price { get; set; }
        public PriceUnitDto PriceUnit { get; set; }

        public static GetAllResourceItemsDTO FromGetAllResourceItems(ResourceUHIA input) =>
            new GetAllResourceItemsDTO
            {
                Id = input.Id,
                ItemAr = input.DescriptorAr,
                ItemEn = input.DescriptorEn,
                Code = input.Code,
                CategoryAr = input.Category.CategoryAr,
                CategoryEn = input.Category.CategoryEn,
                SubCategoryAr = input.SubCategory.SubCategoryAr,
                SubCategoryEn = input.SubCategory.SubCategoryEn,
                Price = ResourceItemPriceDto.FromResourceItemPrice(input.ItemListPrices.OrderByDescending(o => o.EffectiveDateFrom).FirstOrDefault())?.Price,
                PriceUnit = PriceUnitDto.FromPriceUnit(input.ItemListPrices.OrderByDescending(o => o.EffectiveDateFrom).FirstOrDefault()?.PriceUnit)
            };
    }
}
