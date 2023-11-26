using EHealth.ManageItemLists.Application.Facility.UHIA.DTOs;
using EHealth.ManageItemLists.Application.PackageHeaders.DTOs;
using EHealth.ManageItemLists.Domain.Facility.UHIA;
using EHealth.ManageItemLists.Domain.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackageComponent.DTOs
{
    using InvestmentCostPackageComponent = Domain.Packages.InvestmentCostPackage.InvestmentCostPackageComponents.InvestmentCostPackageComponent;

    public class InvestmentCostPackageFacilityUHIADTO
    {
        public Guid? PackageId { get; set; }
        public Guid? FacilityId { get; set; }
        public string? EHealthCode { get; set; }
        public string? DescriptorAr { get; set; }
        public string? DescriptorEn { get; set; }
        public double? OccupancyRate { get; set; }
        public double? OperatingRateInHoursPerDay { get; set; }
        public double? OperatingDaysPerMonth { get; set; }
        public string? CategoryAr { get; set; }
        public string? CategoryEn { get; set; }
        public string? SubCategoryAr { get; set; }
        public string? SubCategoryEn { get; set; }
        public int? QuantityOfUnitsPerTheFacility { get; set; }
        public int? NumberOfSessionsPerUnitPerFacility { get; set; }

        public static InvestmentCostPackageFacilityUHIADTO FromInvestmentCostPackageComponent(InvestmentCostPackageComponent input)
        {
            return new InvestmentCostPackageFacilityUHIADTO
            {
                PackageId = input.Id,
                FacilityId = input.FacilityUHIAId,
                EHealthCode = input?.FacilityUHIA?.Code,
                DescriptorAr = input?.FacilityUHIA?.DescriptorAr,
                DescriptorEn = input?.FacilityUHIA?.DescriptorEn,
                OccupancyRate = input?.FacilityUHIA?.OccupancyRate,
                OperatingRateInHoursPerDay = input?.FacilityUHIA?.OperatingRateInHoursPerDay,
                OperatingDaysPerMonth = input?.FacilityUHIA?.OperatingDaysPerMonth,
                CategoryAr = input?.FacilityUHIA?.Category?.CategoryAr,
                CategoryEn = input?.FacilityUHIA?.Category?.CategoryEn,
                SubCategoryAr = input?.FacilityUHIA?.SubCategory?.SubCategoryAr,
                SubCategoryEn = input?.FacilityUHIA?.SubCategory?.SubCategoryEn,
                QuantityOfUnitsPerTheFacility = input?.QuantityOfUnitsPerTheFacility,
                NumberOfSessionsPerUnitPerFacility = input?.NumberOfSessionsPerUnitPerFacility
            };
        }

    }
}
