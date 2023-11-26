namespace EHealth.ManageItemLists.Application.FeesOfResourcesPerUnitPackageComponent.Recources.DTOs
{
    public class UpdateFeesOfResourcesPerUnitPackageResourceDto
    {
        public Guid Id { get; set; }
        public Guid FeesOfResourcesPerUnitPackageComponentId { get; set; }
        public Guid ResourceUHIAId { get; set; }
        public int Quantity { get; set; }
    }
}
