using EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackagAssets.Commnads;
using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.SharedItemsPackages.SharedItemsPackageConsumablesAndDevices.Commnads.Validators
{
    public class CreateSharedItemsPackageConsumablesAndDevicesCommandValidator : AbstractValidator<CreateSharedItemsPackageConsumablesAndDevicesCommand>
    {
        private readonly ISharedItemsPackageConsumableAndDeviceRepository _sharedItemsPackageConsumableAndDeviceRepository;

        private bool _validInvestmentCostPackageAsset = false;
        public CreateSharedItemsPackageConsumablesAndDevicesCommandValidator(ISharedItemsPackageConsumableAndDeviceRepository sharedItemsPackageConsumableAndDeviceRepository)
        {
            _sharedItemsPackageConsumableAndDeviceRepository = sharedItemsPackageConsumableAndDeviceRepository;
        }
    }
}
