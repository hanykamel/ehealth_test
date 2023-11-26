using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackagAssets;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackagAssets.Commnads.Validators
{
    public class CreateInvestmentCostAssetsCommandValidator : AbstractValidator<CreateInvestmentCostAssetsCommand>
    {
        private readonly IInvestmentCostPackageAssetRepository _investmentCostPackageAssetRepository;
        private readonly IInvestmentCostPackageComponentRepository _investmentCostPackageComponentRepository;
        private readonly IDevicesAndAssetsUHIARepository _devicesAndAssetsUHIARepository;
        private static DevicesAndAssetsUHIA devicesAndAssetsUHIA;

        private bool _validInvestmentCostPackageAsset = false;
        public CreateInvestmentCostAssetsCommandValidator(IInvestmentCostPackageAssetRepository investmentCostPackageAssetRepository,
            IInvestmentCostPackageComponentRepository investmentCostPackageComponentRepository, IDevicesAndAssetsUHIARepository devicesAndAssetsUHIARepository)
        {
            _investmentCostPackageAssetRepository = investmentCostPackageAssetRepository;
            _investmentCostPackageComponentRepository = investmentCostPackageComponentRepository;
            _devicesAndAssetsUHIARepository = devicesAndAssetsUHIARepository;

            RuleFor(x => x.InvestmentCostPackageComponentId).MustAsync(async (InvestmentCostPackageComponentId, CancellationToken) =>
            {
                try
                {
                    var investmentCostPackageComponent = await EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackageComponents.InvestmentCostPackageComponent.Get(InvestmentCostPackageComponentId, _investmentCostPackageComponentRepository);
                    if (investmentCostPackageComponent is not null)
                    {
                        _validInvestmentCostPackageAsset = true;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }).WithErrorCode("InvestmentCostPackageComponentNotExist").WithMessage("InvestmentCostPackageComponent with InvestmentCostPackageComponentId not exist.")
                .When(x => !string.IsNullOrEmpty(x.InvestmentCostPackageComponentId.ToString()));

            RuleFor(x => x.DevicesAndAssetsUHIAId).MustAsync(async (DevicesAndAssetsUHIAId, CancellationToken) =>
            {
                try
                {
                    devicesAndAssetsUHIA = await DevicesAndAssetsUHIA.Get(DevicesAndAssetsUHIAId, _devicesAndAssetsUHIARepository);
                    if (devicesAndAssetsUHIA is not null)
                    {
                        _validInvestmentCostPackageAsset = true;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }).WithErrorCode("DevicesAndAssetsUHIANotExist").WithMessage("DevicesAndAssetsUHIA with DevicesAndAssetsUHIAId not exist.")
                .When(x => !string.IsNullOrEmpty(x.DevicesAndAssetsUHIAId.ToString()));

            var price = devicesAndAssetsUHIA?.ItemListPrices?.FirstOrDefault()?.Price;

            RuleFor(x => x.TotalCost).Equal(e => e.Quantity * price)
                .WithErrorCode("TotalCostNotValid").WithMessage("TotalCost = Quantity * Price.").When(ee => price != null);

            RuleFor(x => x.YearlyDepreciationCostForTheAddedAssets).Equal(e => e.YearlyDepreciationPercentage / 100 * e.TotalCost).WithErrorCode("YearlyDepreciationCostForTheAddedAssetsNotValid")
                .WithMessage("YearlyDepreciationCostForTheAddedAssets = YearlyDepreciationPercentage / 100 * TotalCost.").When(ee => ee.YearlyDepreciationPercentage != null && ee.TotalCost != null);

            RuleFor(x => x.YearlyMaintenanceCostForTheAddedAsset).Equal(e => e.YearlyMaintenancePercentage / 100 * e.TotalCost).WithErrorCode("YearlyMaintenanceCostForTheAddedAssetNotValid")
                .WithMessage("YearlyMaintenanceCostForTheAddedAsset = YearlyMaintenancePercentage / 100 * TotalCost.").When(ee => ee.TotalCost != null);
        }
    }
}
