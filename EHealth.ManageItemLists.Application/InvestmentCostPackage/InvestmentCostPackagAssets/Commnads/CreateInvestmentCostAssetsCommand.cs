using DocumentFormat.OpenXml.Office2010.Excel;
using EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackagAssets.Commnads.Validators;
using EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackagAssets.DTOs;
using EHealth.ManageItemLists.Application.PackageHeaders.DTOs;
using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackageComponents;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackagAssets.Commnads
{
    public class CreateInvestmentCostAssetsCommand : CreateInvestmentCostAssetsDto, IRequest<Guid>, IValidationModel<CreateInvestmentCostAssetsCommand>
    {
        private readonly IInvestmentCostPackageAssetRepository _investmentCostPackageAssetRepository;
        private readonly IInvestmentCostPackageComponentRepository _investmentCostPackageComponentRepository;
        private readonly IDevicesAndAssetsUHIARepository _devicesAndAssetsUHIARepository;

        public CreateInvestmentCostAssetsCommand(CreateInvestmentCostAssetsDto request, IInvestmentCostPackageAssetRepository investmentCostPackageAssetRepository,
            IInvestmentCostPackageComponentRepository investmentCostPackageComponentRepository, IDevicesAndAssetsUHIARepository devicesAndAssetsUHIARepository)
        {
            InvestmentCostPackageComponentId = request.InvestmentCostPackageComponentId;
            DevicesAndAssetsUHIAId = request.DevicesAndAssetsUHIAId;
            Quantity = request.Quantity;
            YearlyDepreciationPercentage = request.YearlyDepreciationPercentage;
            YearlyMaintenancePercentage = request.YearlyMaintenancePercentage;
            TotalCost = request.TotalCost;
            YearlyDepreciationCostForTheAddedAssets = request.YearlyDepreciationCostForTheAddedAssets;
            YearlyMaintenanceCostForTheAddedAsset = request.YearlyMaintenanceCostForTheAddedAsset;
            _investmentCostPackageAssetRepository = investmentCostPackageAssetRepository;
            _investmentCostPackageComponentRepository = investmentCostPackageComponentRepository;
            _devicesAndAssetsUHIARepository = devicesAndAssetsUHIARepository;
        }
        public AbstractValidator<CreateInvestmentCostAssetsCommand> Validator => new CreateInvestmentCostAssetsCommandValidator(_investmentCostPackageAssetRepository, _investmentCostPackageComponentRepository, _devicesAndAssetsUHIARepository);
    }
}
