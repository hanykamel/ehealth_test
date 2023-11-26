using EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackagAssets.Commnads;
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackagAssets;
using EHealth.ManageItemLists.Domain.Packages.SharedItemsPackages.SharedItemsPackageConsumablesAndDevices;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
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

namespace EHealth.ManageItemLists.Application.SharedItemsPackages.SharedItemsPackageConsumablesAndDevices.Commnads.Handlers
{

    public class DeleteSharedItemsPackageConsumablesAndDevicesCommandHandler : IRequestHandler<DeleteSharedItemsPackageConsumablesAndDevicesCommand, bool>
    {
        private readonly ISharedItemsPackageConsumableAndDeviceRepository _sharedItemsPackageConsumableAndDeviceRepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;

        public DeleteSharedItemsPackageConsumablesAndDevicesCommandHandler(ISharedItemsPackageConsumableAndDeviceRepository sharedItemsPackageConsumableAndDeviceRepository, IValidationEngine validationEngine, IIdentityProvider identityProvider)
        {
            _sharedItemsPackageConsumableAndDeviceRepository = sharedItemsPackageConsumableAndDeviceRepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
        }
        public async Task<bool> Handle(DeleteSharedItemsPackageConsumablesAndDevicesCommand request, CancellationToken cancellationToken)
        {
            var sharedItemsPackageConsumableAndDevice = await SharedItemsPackageConsumableAndDevice.Get(request.Id, _sharedItemsPackageConsumableAndDeviceRepository);
            if (sharedItemsPackageConsumableAndDevice is not null)
            {
                sharedItemsPackageConsumableAndDevice.SoftDelete(_identityProvider.GetUserName());

                return (await sharedItemsPackageConsumableAndDevice.Delete(_sharedItemsPackageConsumableAndDeviceRepository, _validationEngine));
            }
            else { throw new DataNotFoundException(); }

        }
    }
}
