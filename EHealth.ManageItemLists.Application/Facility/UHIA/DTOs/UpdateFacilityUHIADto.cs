using EHealth.ManageItemLists.Domain.Facility.UHIA;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Facility.UHIA.DTOs
{
    public class UpdateFacilityUHIADto
    {
        public Guid Id { get; set; }
        public string EHealthCode { get; set; }
        public int? ItemListId { get; set; }
        public string? DescriptorAr { get; set; }
        public string DescriptorEn { get; set; }
        public double? OccupancyRate { get; set; }
        public double OperatingRateInHoursPerDay { get; set; }
        public double OperatingDaysPerMonth { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public DateTime DataEffectiveDateFrom { get; set; }
        public DateTime? DataEffectiveDateTo { get; set; }
        public FacilityUHIA ToFacilityUHIA(string createBy, string telandId) => FacilityUHIA.Create(Id,EHealthCode, DescriptorAr, DescriptorEn, OccupancyRate, OperatingRateInHoursPerDay,
           OperatingDaysPerMonth, CategoryId, SubCategoryId, DataEffectiveDateFrom, DataEffectiveDateTo, createBy, telandId,(int) ItemListId);
    }
}
