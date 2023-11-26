using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.DTOs;
using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Application.Lookups.LocalUnitOfMeasures.DTOs;
using EHealth.ManageItemLists.Application.Lookups.SubCategories.DTOs;

namespace EHealth.ManageItemLists.Application.FeesOfResourcesPerUnitPackageComponent.Recources.DTOs
{
    public class FeesOfResourceSummaryDto
    {
        public int? NumberOfItems { get; set; }
        public int? TotalNumberOfQuantity { get; set; }
        public double? DailyCostOfTotalAddedResourcesPerFacility { get; set; }
        public double? HourlyCostOfTotalAddedResourcesPerFacility { get; set; }
        public double? MinuteCostOfTotalAddedResourcesPerFacility { get; set; }
        public double? DailyCostOfTotalAddedResourcesPerUnit { get; set; }
        public double? HourlyCostOfTotalAddedResourcesPerUnit { get; set; }
        public double? MinuteCostOfTotalAddedResourcesPerUnit { get; set; }
        public double? DailyCostOfTotalAddedResourcesPerSession { get; set; }
        public static FeesOfResourceSummaryDto FromFeesOfResourceSummaryDto(Domain.Packages.FeesOfResourcesPerUnitPackage
            .FeesOfResourcesPerUnitPackageSummaries.FeesOfResourcesPerUnitPackageSummary input) =>
  input is not null ? new FeesOfResourceSummaryDto
  {
      NumberOfItems = input.NumberOfItems,
      TotalNumberOfQuantity = input.TotalNumberOfQuantity,
      DailyCostOfTotalAddedResourcesPerFacility = input.DailyCostOfTotalAddedResourcesPerFacility,
      HourlyCostOfTotalAddedResourcesPerFacility = input.HourlyCostOfTotalAddedResourcesPerFacility,
      MinuteCostOfTotalAddedResourcesPerFacility = input.MinuteCostOfTotalAddedResourcesPerFacility,
      DailyCostOfTotalAddedResourcesPerUnit = input.DailyCostOfTotalAddedResourcesPerUnit,
      HourlyCostOfTotalAddedResourcesPerUnit = input.HourlyCostOfTotalAddedResourcesPerUnit,
      MinuteCostOfTotalAddedResourcesPerUnit = input.MinuteCostOfTotalAddedResourcesPerUnit,
      DailyCostOfTotalAddedResourcesPerSession = input.DailyCostOfTotalAddedResourcesPerSession,


  }:null;
    }
}
