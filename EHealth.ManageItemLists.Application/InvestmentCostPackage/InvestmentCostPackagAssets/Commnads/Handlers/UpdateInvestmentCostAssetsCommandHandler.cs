using EHealth.ManageItemLists.Application.PackageHeaders.Commands;
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostDepreciationsAndMaintenances;
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackagAssets;
using EHealth.ManageItemLists.Domain.Packages.PackageHeaders;
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
    public class UpdateInvestmentCostAssetsCommandHandler : IRequestHandler<UpdateInvestmentCostAssetsCommand, bool>
    {
        private readonly IInvestmentCostPackageAssetRepository _investmentCostPackageAssetRepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IInvestmentCostPackageComponentRepository _investmentCostPackageComponentRepository;
        private readonly IInvestmentCostDepreciationAndMaintenanceRepository _investmentCostDepreciationAndMaintenanceRepository;
        private readonly IIdentityProvider _identityProvider;

        public UpdateInvestmentCostAssetsCommandHandler(IInvestmentCostPackageAssetRepository investmentCostPackageAssetRepository,
        IValidationEngine validationEngine,
        IInvestmentCostPackageComponentRepository investmentCostPackageComponentRepository, IInvestmentCostDepreciationAndMaintenanceRepository investmentCostDepreciationAndMaintenanceRepository,
        IIdentityProvider identityProvider)
        {
            _investmentCostPackageAssetRepository = investmentCostPackageAssetRepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
            _investmentCostPackageComponentRepository = investmentCostPackageComponentRepository;
            _investmentCostDepreciationAndMaintenanceRepository = investmentCostDepreciationAndMaintenanceRepository;
        }
        public async Task<bool> Handle(UpdateInvestmentCostAssetsCommand request, CancellationToken cancellationToken)
        {
            //validate model
            _validationEngine.Validate(request);

            var investmentCostPackageAsset = await InvestmentCostPackageAsset.Get(request.Id, _investmentCostPackageAssetRepository);

            investmentCostPackageAsset.SetDevicesAndAssetsUHIAId(request.DevicesAndAssetsUHIAId);
            investmentCostPackageAsset.SetInvestmentCostPackageComponentId(request.InvestmentCostPackageComponentId);
            investmentCostPackageAsset.SetQuantity(request.Quantity);
            investmentCostPackageAsset.SetTotalCost(request.TotalCost);
            investmentCostPackageAsset.SetYearlyDepreciationCostForTheAddedAssets(request.YearlyDepreciationCostForTheAddedAssets);
            investmentCostPackageAsset.SetYearlyDepreciationPercentage(request.YearlyDepreciationPercentage);
            investmentCostPackageAsset.SetYearlyMaintenanceCostForTheAddedAsset(request.YearlyMaintenanceCostForTheAddedAsset);
            investmentCostPackageAsset.SetYearlyMaintenancePercentage(request.YearlyMaintenancePercentage);

            investmentCostPackageAsset.SetModifiedBy(_identityProvider.GetUserName());
            investmentCostPackageAsset.SetModifiedOn();
            var res = await investmentCostPackageAsset.Update(_investmentCostPackageAssetRepository, _validationEngine);

            var investmentCostPackageComponent = await _investmentCostPackageComponentRepository.Get(request.InvestmentCostPackageComponentId);
            var investmentCostPackageAssetList = await _investmentCostPackageAssetRepository.Search(ee => ee.InvestmentCostPackageComponentId == investmentCostPackageComponent.Id, 1, 1, false, null, null);
            var facility = investmentCostPackageComponent?.FacilityUHIA;
            var calculateFields = await InvestmentCostDepreciationAndMaintenance.CalculateFields(investmentCostPackageComponent.InvestmentCostDepreciationAndMaintenanceId.Value
                , investmentCostPackageAssetList.Data, investmentCostPackageComponent, facility, _investmentCostDepreciationAndMaintenanceRepository, _validationEngine,
                _identityProvider.GetUserName(), _identityProvider.GetTenantId());

            return res;
        }
    }
}
