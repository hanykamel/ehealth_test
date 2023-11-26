using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostDepreciationsAndMaintenances;
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackageComponents;
using EHealth.ManageItemLists.Domain.Shared.Identity;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackageComponent.Commnads.Handlers
{
    using InvestmentCostPackageComponent = Domain.Packages.InvestmentCostPackage.InvestmentCostPackageComponents.InvestmentCostPackageComponent;
    using FacilityUHIA = Domain.Facility.UHIA.FacilityUHIA;

    public class SetInvestmentCostComponentHandler : IRequestHandler<SetInvestmentCostComponentCommand, Guid>
    {
        private readonly IIdentityProvider _identityProvider;
        private readonly IInvestmentCostPackageComponentRepository _investmentCostPackageComponentRepository;
        private readonly IInvestmentCostDepreciationAndMaintenanceRepository _investmentCostDepreciationAndMaintenanceRepository;
        private readonly IFacilityUHIARepository _facilityUHIARepository;
        private readonly IValidationEngine _validationEngine;

        public SetInvestmentCostComponentHandler(IIdentityProvider identityProvider,
            IInvestmentCostPackageComponentRepository investmentCostPackageComponentRepository,
            IInvestmentCostDepreciationAndMaintenanceRepository investmentCostDepreciationAndMaintenanceRepository,
            IValidationEngine validationEngine,
            IFacilityUHIARepository facilityUHIARepository)
        {
            _identityProvider = identityProvider;
            _investmentCostPackageComponentRepository = investmentCostPackageComponentRepository;
            _investmentCostDepreciationAndMaintenanceRepository = investmentCostDepreciationAndMaintenanceRepository;
            _validationEngine = validationEngine;
            _facilityUHIARepository = facilityUHIARepository;
        }
        public async Task<Guid> Handle(SetInvestmentCostComponentCommand request, CancellationToken cancellationToken)
        {
            _validationEngine.Validate(request);

            var investmentCosts = await InvestmentCostPackageComponent.Search(_investmentCostPackageComponentRepository, p => p.PackageHeaderId == request.PackageHeaderId, 1, 1, false, null, null);
            var investmentCost = investmentCosts.Data.FirstOrDefault();
            if (investmentCost is null)
            {
                investmentCost = request.ToInvestmentCostComponent(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
                await investmentCost.Create(_investmentCostPackageComponentRepository, _validationEngine);
                var facility = await FacilityUHIA.Get(request.FacilityUHIAId, _facilityUHIARepository);
                var InvestmentCostDepreciationAndMaintenanceId = await InvestmentCostDepreciationAndMaintenance.CalculateFields(Guid.Empty,
                investmentCost.InvestmentCostPackagAssets,
                investmentCost, facility, _investmentCostDepreciationAndMaintenanceRepository, _validationEngine, _identityProvider.GetUserName(), _identityProvider.GetTenantId());
                investmentCost.SetInvestmentCostDepreciationAndMaintenanceId(InvestmentCostDepreciationAndMaintenanceId);
                await investmentCost.Update(_investmentCostPackageComponentRepository, _validationEngine);
            }
            else
            {
                investmentCost = investmentCosts.Data.FirstOrDefault();
                investmentCost.SetFacilityUHIA(request.FacilityUHIAId);
                investmentCost.SetNumberOfSessionsPerUnitPerFacility(request.NumberOfSessionsPerUnitPerFacility);
                investmentCost.SetQuantityOfUnitsPerTheFacility(request.QuantityOfUnitsPerTheFacility);
                await investmentCost.Update(_investmentCostPackageComponentRepository, _validationEngine);
                var facility = await FacilityUHIA.Get(request.FacilityUHIAId, _facilityUHIARepository);
                await InvestmentCostDepreciationAndMaintenance.CalculateFields(investmentCost.InvestmentCostDepreciationAndMaintenanceId.Value,
                investmentCost.InvestmentCostPackagAssets,
                investmentCost, facility, _investmentCostDepreciationAndMaintenanceRepository, _validationEngine, _identityProvider.GetUserName(), _identityProvider.GetTenantId());
                await investmentCost.Update(_investmentCostPackageComponentRepository, _validationEngine);
            }
            return investmentCost.Id;
        }
    }
}
