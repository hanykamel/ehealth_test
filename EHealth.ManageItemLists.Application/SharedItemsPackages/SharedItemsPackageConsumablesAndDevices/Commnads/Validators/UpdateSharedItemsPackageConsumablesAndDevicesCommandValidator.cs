using EHealth.ManageItemLists.Domain.Shared.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.SharedItemsPackages.SharedItemsPackageConsumablesAndDevices.Commnads.Validators
{
    internal class UpdateSharedItemsPackageConsumablesAndDevicesCommandValidator : AbstractValidator<UpdateSharedItemsPackageConsumablesAndDevicesCommand>
    {
        private readonly ISharedItemsPackageConsumableAndDeviceRepository _sharedItemsPackageConsumableAndDeviceRepository;

        private bool _validInvestmentCostPackageAsset = false;
        public UpdateSharedItemsPackageConsumablesAndDevicesCommandValidator(ISharedItemsPackageConsumableAndDeviceRepository sharedItemsPackageConsumableAndDeviceRepository)
        {
            _sharedItemsPackageConsumableAndDeviceRepository = sharedItemsPackageConsumableAndDeviceRepository;
        }
    }
}
