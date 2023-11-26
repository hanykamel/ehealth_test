using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.SubCategories.DTOs;
using EHealth.ManageItemLists.Domain.Facility.UHIA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Facility.UHIA.DTOs
{
    public class GetAllFacilitiesDTO
    {
        public Guid Id { get; private set; }
        public string? DescriptorAr { get; private set; }
        public string DescriptorEn { get; private set; }
        public string? EHealthCode { get; private set; }
        public double? OccupancyRate { get; set; }
        public string CategoryAr { get; set; }
        public string CategoryEn { get; set; }
        public string SubCategoryAr { get; set; }
        public string SubCategoryEn { get; set; }
        public DateTime? DateFrom { get;  set; }
        public DateTime? DateTo { get; set; }

        public static GetAllFacilitiesDTO FromFacilityUHIA(FacilityUHIA input) =>
            new GetAllFacilitiesDTO
            {
                Id = input.Id,
                DescriptorAr = input.DescriptorAr,
                DescriptorEn = input.DescriptorEn,
                EHealthCode=input.Code,
                OccupancyRate=input.OccupancyRate,
                CategoryEn=input.Category.CategoryEn,
                CategoryAr=input.Category.CategoryEn,
                SubCategoryAr=input.SubCategory.SubCategoryAr,
                SubCategoryEn=input.SubCategory.SubCategoryEn,
                DateFrom = input.DataEffectiveDateFrom,
                DateTo = input.DataEffectiveDateTo
                
            };
    }
}
