namespace EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackageComponent.DTOs
{
    using InvestmentCostPackageComponent = Domain.Packages.InvestmentCostPackage.InvestmentCostPackageComponents.InvestmentCostPackageComponent;

    public class SetInvestmentCostComponentDto
    {
        public Guid PackageHeaderId { get; set; }
        public Guid FacilityUHIAId { get; set; }
        public int QuantityOfUnitsPerTheFacility { get; set; }
        public int NumberOfSessionsPerUnitPerFacility { get; set; }

        public InvestmentCostPackageComponent ToInvestmentCostComponent(string createdBy, string tenantId)
        {
            return InvestmentCostPackageComponent.Create(null, PackageHeaderId, FacilityUHIAId, QuantityOfUnitsPerTheFacility, NumberOfSessionsPerUnitPerFacility, null, createdBy, tenantId);
        }
    }
}
