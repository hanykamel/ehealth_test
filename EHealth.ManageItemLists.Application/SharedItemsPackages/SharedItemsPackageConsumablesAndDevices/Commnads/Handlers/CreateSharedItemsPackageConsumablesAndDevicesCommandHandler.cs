using EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackagAssets.Commnads;
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostDepreciationsAndMaintenances;
using EHealth.ManageItemLists.Domain.Shared.Identity;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.SharedItemsPackages.SharedItemsPackageConsumablesAndDevices.Commnads.Handlers
{
    public class CreateSharedItemsPackageConsumablesAndDevicesCommandHandler : IRequestHandler<CreateSharedItemsPackageConsumablesAndDevicesCommand, Guid>
    {
        private readonly IValidationEngine _validationEngine;
        private readonly ISharedItemsPackageConsumableAndDeviceRepository _sharedItemsPackageConsumableAndDeviceRepository;
        private readonly IIdentityProvider _identityProvider;

        public CreateSharedItemsPackageConsumablesAndDevicesCommandHandler(IValidationEngine validationEngine,
            ISharedItemsPackageConsumableAndDeviceRepository sharedItemsPackageConsumableAndDeviceRepository, IIdentityProvider identityProvider)
        {
            _validationEngine = validationEngine;
            _sharedItemsPackageConsumableAndDeviceRepository = sharedItemsPackageConsumableAndDeviceRepository;
            _identityProvider = identityProvider;
        }

        public async Task<Guid> Handle(CreateSharedItemsPackageConsumablesAndDevicesCommand request, CancellationToken cancellationToken)
        {
            //validate model
            _validationEngine.Validate(request);

            var sharedItemsPackageConsumableAndDevice = request.ToSharedItemsPackageConsumableAndDevice(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
            await sharedItemsPackageConsumableAndDevice.Create(_sharedItemsPackageConsumableAndDeviceRepository, _validationEngine);
            
            return sharedItemsPackageConsumableAndDevice.Id;
        }
    }
}
