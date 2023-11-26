using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackagAssets.Commnads.Validators;
using EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackagAssets.DTOs;
using EHealth.ManageItemLists.Application.PackageHeaders.Commands.Validators;
using EHealth.ManageItemLists.Application.PackageHeaders.DTOs;
using EHealth.ManageItemLists.Domain.GlobelPackageTypes;
using EHealth.ManageItemLists.Domain.PackageComplexityClassifications;
using EHealth.ManageItemLists.Domain.PackageSpecialties;
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
    public class UpdateInvestmentCostAssetsCommand : UpdateInvestmentCostAssetsDto, IRequest<bool>, IValidationModel<UpdateInvestmentCostAssetsCommand>
    {
      
        private readonly IInvestmentCostPackageAssetRepository _investmentCostPackageAssetRepository;
        private readonly IInvestmentCostPackageComponentRepository _investmentCostPackageComponentRepository;
        private readonly IDevicesAndAssetsUHIARepository _devicesAndAssetsUHIARepository;

        public UpdateInvestmentCostAssetsCommand(UpdateInvestmentCostAssetsDto request, IInvestmentCostPackageAssetRepository investmentCostPackageAssetRepository,
            IInvestmentCostPackageComponentRepository investmentCostPackageComponentRepository, IDevicesAndAssetsUHIARepository devicesAndAssetsUHIARepository)
        {
            Id = request.Id;
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

        public AbstractValidator<UpdateInvestmentCostAssetsCommand> Validator => new UpdateInvestmentCostAssetsCommandValidator(_investmentCostPackageAssetRepository, _investmentCostPackageComponentRepository, _devicesAndAssetsUHIARepository);
    }
}
