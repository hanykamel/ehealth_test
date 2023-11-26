using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostDepreciationsAndMaintenances;
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackagAssets;
using EHealth.ManageItemLists.Domain.Packages.SharedItemsPackages.SharedItemsPackageConsumablesAndDevices;
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
    public class UpdateSharedItemsPackageConsumablesAndDevicesCommandHandler : IRequestHandler<UpdateSharedItemsPackageConsumablesAndDevicesCommand, bool>
    {
        private readonly IValidationEngine _validationEngine;
        private readonly ISharedItemsPackageConsumableAndDeviceRepository _sharedItemsPackageConsumableAndDeviceRepository;
        private readonly IIdentityProvider _identityProvider;

        public UpdateSharedItemsPackageConsumablesAndDevicesCommandHandler(IValidationEngine validationEngine,
            ISharedItemsPackageConsumableAndDeviceRepository sharedItemsPackageConsumableAndDeviceRepository, IIdentityProvider identityProvider)
        {
            _validationEngine = validationEngine;
            _sharedItemsPackageConsumableAndDeviceRepository = sharedItemsPackageConsumableAndDeviceRepository;
            _identityProvider = identityProvider;
        }

        public async Task<bool> Handle(UpdateSharedItemsPackageConsumablesAndDevicesCommand request, CancellationToken cancellationToken)
        {
            //validate model
            _validationEngine.Validate(request);


            var sharedItemsPackageConsumableAndDevice = await SharedItemsPackageConsumableAndDevice.Get(request.Id, _sharedItemsPackageConsumableAndDeviceRepository);

            sharedItemsPackageConsumableAndDevice.SetConsumablesAndDevicesUHIAId(request.ConsumablesAndDevicesUHIAId);
            sharedItemsPackageConsumableAndDevice.SetSharedItemsPackageComponentId(request.SharedItemsPackageComponentId);
            sharedItemsPackageConsumableAndDevice.SetQuantity(request.Quantity);
            sharedItemsPackageConsumableAndDevice.SetTotalCost(request.TotalCost);
            sharedItemsPackageConsumableAndDevice.SetConsumablePerCase(request.ConsumablePerCase);
            sharedItemsPackageConsumableAndDevice.SetNumberOfCasesInTheUnit(request.NumberOfCasesInTheUnit);
            sharedItemsPackageConsumableAndDevice.SetLocationId(request.LocationId);

            sharedItemsPackageConsumableAndDevice.SetModifiedBy(_identityProvider.GetUserName());
            sharedItemsPackageConsumableAndDevice.SetModifiedOn();
            var res = await sharedItemsPackageConsumableAndDevice.Update(_sharedItemsPackageConsumableAndDeviceRepository, _validationEngine);

            return res;
        }
    }
}
