using EHealth.ManageItemLists.Application.SharedItemsPackages.SharedItemsPackageConsumablesAndDevices.Commnads.Validators;
using EHealth.ManageItemLists.Application.SharedItemsPackages.SharedItemsPackageConsumablesAndDevices.DTOs;
using EHealth.ManageItemLists.Domain.ConsumablesAndDevices;
using EHealth.ManageItemLists.Domain.Packages.SharedItemsPackages.SharedItemsPackageComponents;
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
    public class UpdateSharedItemsPackageConsumablesAndDevicesCommand : UpdateSharedItemsPackageConsumablesAndDevicesDto, IRequest<bool>, IValidationModel<UpdateSharedItemsPackageConsumablesAndDevicesCommand>
    {
        private readonly ISharedItemsPackageConsumableAndDeviceRepository _sharedItemsPackageConsumableAndDeviceRepository;

        public UpdateSharedItemsPackageConsumablesAndDevicesCommand(UpdateSharedItemsPackageConsumablesAndDevicesDto request, ISharedItemsPackageConsumableAndDeviceRepository sharedItemsPackageConsumableAndDeviceRepository)
        {
            Id = request.Id;
            SharedItemsPackageComponentId = request.SharedItemsPackageComponentId;
            ConsumablesAndDevicesUHIAId = request.ConsumablesAndDevicesUHIAId;
            Quantity = request.Quantity;
            NumberOfCasesInTheUnit = request.NumberOfCasesInTheUnit;
            LocationId = request.LocationId;
            TotalCost = request.TotalCost;
            ConsumablePerCase = request.ConsumablePerCase;

            _sharedItemsPackageConsumableAndDeviceRepository = sharedItemsPackageConsumableAndDeviceRepository;
        }
        public AbstractValidator<UpdateSharedItemsPackageConsumablesAndDevicesCommand> Validator => new UpdateSharedItemsPackageConsumablesAndDevicesCommandValidator(_sharedItemsPackageConsumableAndDeviceRepository);
    }
}
