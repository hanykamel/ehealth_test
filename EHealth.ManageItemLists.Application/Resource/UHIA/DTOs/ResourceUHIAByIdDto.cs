using EHealth.ManageItemLists.Application.DoctorFees.ItemPrice.DTOs;
using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.SubCategories.DTOs;
using EHealth.ManageItemLists.Application.Resource.ItemPrice.DTOs;
using EHealth.ManageItemLists.Domain.Resource.UHIA;

namespace EHealth.ManageItemLists.Application.Resource.UHIA.DTOs
{
    public class ResourceUHIAByIdDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string? DescriptorAr { get; set; }
        public string DescriptorEn { get; set; }
        public CategoryDto Category { get; set; }
        public SubCategoryDto SubCategory { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int ItemListId { get; set; }
        public string DataEffectiveDateFrom { get; set; }
        public string? DataEffectiveDateTo { get; set; }
        public IList<ResourceItemPriceDto> ItemListPrices { get; set; }=new List<ResourceItemPriceDto>();
        public string? ModifiedBy { get; set; }
        public string? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }

        public static ResourceUHIAByIdDto FromResourceUHIA(ResourceUHIA input) =>
     new ResourceUHIAByIdDto
     {
         Id = input.Id,
         Code = input.Code,
         DescriptorAr = input.DescriptorAr,
         DescriptorEn = input.DescriptorEn,
         Category = CategoryDto.FromCategory(input.Category),
         SubCategory = SubCategoryDto.FromSubCategory(input.SubCategory),
         DataEffectiveDateFrom = input.DataEffectiveDateFrom.ToString("yyyy-MM-dd"),
         DataEffectiveDateTo = input.DataEffectiveDateTo?.ToString("yyyy-MM-dd"),
         ItemListPrices =input.ItemListPrices.Select(p=> ResourceItemPriceDto.FromResourceItemPrice(p)).ToList(),
         ModifiedBy = input.ModifiedBy,
         ModifiedOn = input.ModifiedOn?.ToString("yyyy-MM-dd hh:mm"),
         CategoryId = input.CategoryId,
         ItemListId = input.ItemListId,
         SubCategoryId = input.SubCategoryId,
         IsDeleted = input.IsDeleted
     };
    }
}
