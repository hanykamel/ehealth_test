using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.SubCategories.DTOs;
using EHealth.ManageItemLists.Domain.Facility.UHIA;

namespace EHealth.ManageItemLists.Application.Facility.UHIA.DTOs
{
    public class FacilityUHIADto
    {
        public Guid Id { get; set; }
        public string? EHealthCode { get; private set; }
        public string? DescriptorAr { get; set; }
        public string DescriptorEn { get; set; }
        public double? OccupancyRate { get; set; }
        public double OperatingRateInHoursPerDay { get; set; }
        public double OperatingDaysPerMonth { get; set; }
        public CategoryDto Category { get; set; }
        public SubCategoryDto SubCategory { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int ItemListId { get; set; }
      
        public string DataEffectiveDateFrom { get; private set; }
        public string? DataEffectiveDateTo { get; private set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }

        public static FacilityUHIADto FromFacilityUHIA(FacilityUHIA input) =>
        new FacilityUHIADto
        {
            Id = input.Id,
            EHealthCode = input.Code,
            DescriptorAr = input.DescriptorAr,
            DescriptorEn = input.DescriptorEn,
            OccupancyRate = input.OccupancyRate,
            OperatingRateInHoursPerDay = input.OperatingRateInHoursPerDay,
            OperatingDaysPerMonth = input.OperatingDaysPerMonth,
            Category = CategoryDto.FromCategory(input.Category),
            SubCategory = SubCategoryDto.FromSubCategory(input.SubCategory),
            DataEffectiveDateFrom = input.DataEffectiveDateFrom.ToString("yyyy-MM-dd"),
            DataEffectiveDateTo = input.DataEffectiveDateTo?.ToString("yyyy-MM-dd"),
            ModifiedBy = input.ModifiedBy,
            ModifiedOn = input.ModifiedOn?.ToString("yyyy-MM-dd"),
            ItemListId = input.ItemListId,
            CategoryId = input.CategoryId,
            SubCategoryId = input.SubCategoryId,
            IsDeleted = input.IsDeleted
        };
    }
}
