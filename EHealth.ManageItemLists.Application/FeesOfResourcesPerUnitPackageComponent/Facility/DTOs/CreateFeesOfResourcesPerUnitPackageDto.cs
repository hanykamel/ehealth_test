namespace EHealth.ManageItemLists.Application.FeesOfResourcesPerUnitPackageComponent.Facility.DTOs
{
    public class CreateFeesOfResourcesPerUnitPackageDto
    {
        //public Guid? Id { get; set; }
        public Guid FacilityUHIAId { get; set; }
        public Guid PackageHeaderId { get; set; }
        public int? QuantityOfUnitsPerTheFacility { get; set; }
        public int? NumberOfSessionsPerUnitPerFacility { get; set; }
        public Domain.Packages.FeesOfResourcesPerUnitPackage.FeesOfResourcesPerUnitPackageComponents.FeesOfResourcesPerUnitPackageComponent ToFeesOfResourcesPerUnitPackage(string createdBy, string tenantId,Guid? feesOfResourcesPerUnitPackageSummaryId) =>
          Domain.Packages.FeesOfResourcesPerUnitPackage.FeesOfResourcesPerUnitPackageComponents.FeesOfResourcesPerUnitPackageComponent.Create(null,PackageHeaderId, FacilityUHIAId
           , QuantityOfUnitsPerTheFacility, NumberOfSessionsPerUnitPerFacility, feesOfResourcesPerUnitPackageSummaryId, createdBy, tenantId);
    }
}
