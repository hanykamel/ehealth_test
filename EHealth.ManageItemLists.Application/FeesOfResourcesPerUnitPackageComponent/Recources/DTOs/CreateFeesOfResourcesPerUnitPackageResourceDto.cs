namespace EHealth.ManageItemLists.Application.FeesOfResourcesPerUnitPackageComponent.Recources.DTOs
{
    public class CreateFeesOfResourcesPerUnitPackageResourceDto
    {
        public Guid FeesOfResourcesPerUnitPackageComponentId { get; set; }
        public Guid ResourceUHIAId { get;  set; }
        public int Quantity { get;  set; }

        public Domain.Packages.FeesOfResourcesPerUnitPackage.FeesOfResourcesPerUnitPackageResources.FeesOfResourcesPerUnitPackageResource ToFeesOfResourcesPerUnitPackageResource
            (string createdBy, string tenantId,double? DailyCostOfTheResource,double? TotalDailyCostOfResourcePerFacility) => Domain.Packages.FeesOfResourcesPerUnitPackage.FeesOfResourcesPerUnitPackageResources.FeesOfResourcesPerUnitPackageResource
            .Create(null, FeesOfResourcesPerUnitPackageComponentId, ResourceUHIAId, DailyCostOfTheResource, Quantity, TotalDailyCostOfResourcePerFacility, createdBy, tenantId);
    }
}
