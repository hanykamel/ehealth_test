using EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackagAssets.Commnads.Validators;
using EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackagAssets.DTOs;
using EHealth.ManageItemLists.Application.SharedItemsPackages.SharedItemsPackageConsumablesAndDevices.Commnads.Validators;
using EHealth.ManageItemLists.Application.SharedItemsPackages.SharedItemsPackageConsumablesAndDevices.DTOs;
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

namespace EHealth.ManageItemLists.Application.SharedItemsPackages.SharedItemsPackageConsumablesAndDevices.Commnads
{
    public class CreateSharedItemsPackageConsumablesAndDevicesCommand : CreateSharedItemsPackageConsumablesAndDevicesDto, IRequest<Guid>, IValidationModel<CreateSharedItemsPackageConsumablesAndDevicesCommand>
    {
        private readonly ISharedItemsPackageConsumableAndDeviceRepository _sharedItemsPackageConsumableAndDeviceRepository;

        public CreateSharedItemsPackageConsumablesAndDevicesCommand(CreateSharedItemsPackageConsumablesAndDevicesDto request, ISharedItemsPackageConsumableAndDeviceRepository sharedItemsPackageConsumableAndDeviceRepository)
        {
            SharedItemsPackageComponentId = request.SharedItemsPackageComponentId;
            ConsumablesAndDevicesUHIAId = request.ConsumablesAndDevicesUHIAId;
            Quantity = request.Quantity;
            NumberOfCasesInTheUnit = request.NumberOfCasesInTheUnit;
            LocationId = request.LocationId;
            TotalCost = request.TotalCost;
            ConsumablePerCase = request.ConsumablePerCase;

            _sharedItemsPackageConsumableAndDeviceRepository = sharedItemsPackageConsumableAndDeviceRepository;
        }
        public AbstractValidator<CreateSharedItemsPackageConsumablesAndDevicesCommand> Validator => new CreateSharedItemsPackageConsumablesAndDevicesCommandValidator(_sharedItemsPackageConsumableAndDeviceRepository);
    }
}
