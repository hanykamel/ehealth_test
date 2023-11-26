using EHealth.ManageItemLists.Application.Facility.UHIA.DTOs;
using EHealth.ManageItemLists.Application.ItemListPrices.DTOs;
using EHealth.ManageItemLists.Application.Resource.ItemPrice.DTOs;
using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using EHealth.ManageItemLists.Domain.Facility.UHIA;
using EHealth.ManageItemLists.Domain.ItemListPricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.PackageItems.DTOs
{
    public class GetAllAssetsAndMedicalEquipmentsDTO
    {
        public Guid Id { get; private set; }
        public string? DescriptorAr { get; private set; }
        public string DescriptorEn { get; private set; }
        public string? EHealthCode { get; private set; }
        public string? CategoryAr { get; set; }
        public string? CategoryEn { get; set; }
        public string? SubCategoryAr { get; set; }
        public string? SubCategoryEn { get; set; }
        public double? Price { get; set; }

        public static GetAllAssetsAndMedicalEquipmentsDTO FromAssetsAndMedicalEquipment(DevicesAndAssetsUHIA input) =>
            new GetAllAssetsAndMedicalEquipmentsDTO
            {
                Id = input.Id,
                DescriptorAr = input.DescriptorAr,
                DescriptorEn = input.DescriptorEn,
                EHealthCode = input.Code,
                CategoryEn = input.Category?.CategoryEn,
                CategoryAr = input.Category?.CategoryEn,
                SubCategoryAr = input.SubCategory.SubCategoryAr,
                SubCategoryEn = input.SubCategory.SubCategoryEn,
                Price = ItemListPriceDto.FromItemListPrice(input.ItemListPrices.OrderByDescending(e => e.EffectiveDateFrom).FirstOrDefault())?.Price
            };
    }
}
