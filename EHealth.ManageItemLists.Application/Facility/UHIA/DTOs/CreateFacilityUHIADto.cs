using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.SubCategories.DTOs;
using EHealth.ManageItemLists.Domain.Facility.UHIA;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.ItemListSubtypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Facility.UHIA.DTOs
{
    public class CreateFacilityUHIADto
    {
        public string Code { get; set; }
        public string? DescriptorAr { get; set; }
        public string DescriptorEn { get; set; }
        public double? OccupancyRate { get; set; }
        public double OperatingRateInHoursPerDay { get; set; }
        public double OperatingDaysPerMonth { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public DateTime DataEffectiveDateFrom { get; set; }
        public DateTime? DataEffectiveDateTo { get; set; }
        public int ItemListId { get; set; }

        public FacilityUHIA ToFacilityUHIA(string createBy, string telandId) => FacilityUHIA.Create(null,Code, DescriptorAr, DescriptorEn, OccupancyRate, OperatingRateInHoursPerDay, 
            OperatingDaysPerMonth, CategoryId, SubCategoryId, DataEffectiveDateFrom, DataEffectiveDateTo, createBy, telandId, ItemListId);
    }
}
