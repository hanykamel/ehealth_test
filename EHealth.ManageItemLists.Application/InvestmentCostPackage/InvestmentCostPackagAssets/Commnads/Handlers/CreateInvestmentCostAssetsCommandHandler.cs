using EHealth.ManageItemLists.Application.PackageHeaders.Commands;
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostDepreciationsAndMaintenances;
using EHealth.ManageItemLists.Domain.Shared.Identity;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using EHealth.ManageItemLists.Infrastructure.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackagAssets.Commnads.Handlers
{

    public class CreateInvestmentCostAssetsCommandHandler : IRequestHandler<CreateInvestmentCostAssetsCommand, Guid>
    {
        private readonly IValidationEngine _validationEngine;
        private readonly IInvestmentCostPackageAssetRepository _investmentCostPackageAssetRepository;
        private readonly IInvestmentCostPackageComponentRepository _investmentCostPackageComponentRepository;
        private readonly IInvestmentCostDepreciationAndMaintenanceRepository _investmentCostDepreciationAndMaintenanceRepository;
        private readonly IIdentityProvider _identityProvider;

        public CreateInvestmentCostAssetsCommandHandler(IValidationEngine validationEngine, IInvestmentCostPackageAssetRepository investmentCostPackageAssetRepository,
            IInvestmentCostPackageComponentRepository investmentCostPackageComponentRepository, IInvestmentCostDepreciationAndMaintenanceRepository investmentCostDepreciationAndMaintenanceRepository,
            IIdentityProvider identityProvider)
        {
            _validationEngine = validationEngine;
            _investmentCostPackageAssetRepository = investmentCostPackageAssetRepository;
            _investmentCostPackageComponentRepository = investmentCostPackageComponentRepository;
            _investmentCostDepreciationAndMaintenanceRepository = investmentCostDepreciationAndMaintenanceRepository;
            _identityProvider = identityProvider;
        }

        public async Task<Guid> Handle(CreateInvestmentCostAssetsCommand request, CancellationToken cancellationToken)
        {
            //validate model
            _validationEngine.Validate(request);

            var investmentCostPackageAsset = request.ToInvestmentCostPackageAsset(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
            await investmentCostPackageAsset.Create(_investmentCostPackageAssetRepository, _validationEngine);
            var investmentCostPackageComponent = await _investmentCostPackageComponentRepository.Get(request.InvestmentCostPackageComponentId);
            var investmentCostPackageAssetList = await _investmentCostPackageAssetRepository.Search(ee => ee.InvestmentCostPackageComponentId == investmentCostPackageComponent.Id, 1, 1, false, null, null);
            var facility = investmentCostPackageComponent?.FacilityUHIA;
            var calculateFields = await InvestmentCostDepreciationAndMaintenance.CalculateFields(investmentCostPackageComponent.InvestmentCostDepreciationAndMaintenanceId.Value
                , investmentCostPackageAssetList.Data, investmentCostPackageComponent, facility, _investmentCostDepreciationAndMaintenanceRepository, _validationEngine,
                _identityProvider.GetUserName(), _identityProvider.GetTenantId());

            return investmentCostPackageAsset.Id;
        }
    }
}
