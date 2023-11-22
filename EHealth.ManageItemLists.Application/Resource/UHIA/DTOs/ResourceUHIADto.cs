using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.SubCategories.DTOs;
using EHealth.ManageItemLists.Application.Resource.ItemPrice.DTOs;
using EHealth.ManageItemLists.Domain.Resource.UHIA;

namespace EHealth.ManageItemLists.Application.Resource.UHIA.DTOs
{
    public class ResourceUHIADto
    {
        public Guid Id { get; set; }
        public string EHealthCode { get; set; }
        public string? DescriptorAr { get; set; }
        public string DescriptorEn { get; set; }
        public CategoryDto Category { get; set; }
        public SubCategoryDto SubCategory { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int ItemListId { get; set; }
        public string DataEffectiveDateFrom { get; set; }
        public string? DataEffectiveDateTo { get; set; }
        public ResourceItemPriceDto ItemListPrice { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }

        public static ResourceUHIADto FromResourceUHIA(ResourceUHIA input) =>
        new ResourceUHIADto
        {
            Id = input.Id,
            EHealthCode = input.Code,
            DescriptorAr = input.DescriptorAr,
            DescriptorEn = input.DescriptorEn,
            Category = CategoryDto.FromCategory(input.Category),
            SubCategory = SubCategoryDto.FromSubCategory(input.SubCategory),
            DataEffectiveDateFrom = input.DataEffectiveDateFrom.ToString("yyyy-MM-dd"),
            DataEffectiveDateTo = input.DataEffectiveDateTo?.ToString("yyyy-MM-dd"),
            ModifiedBy = input.ModifiedBy,
            ModifiedOn = input.ModifiedOn?.ToString("yyyy-MM-dd"),
            CategoryId = input.CategoryId,
            ItemListId = input.ItemListId,
            SubCategoryId = input.SubCategoryId,
            ItemListPrice = ResourceItemPriceDto.FromResourceItemPrice(input.ItemListPrices.OrderByDescending(e => e.EffectiveDateFrom).FirstOrDefault()),
            IsDeleted = input.IsDeleted
        };
    }
}
