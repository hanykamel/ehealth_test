using EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA;
using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.Commands.Handler
{
    public class DeleteDevicesAndAssetsUHIACommandHandler : IRequestHandler<DeleteDevicesAndAssetsUHIACommand, bool>
    {
        private readonly IDevicesAndAssetsUHIARepository _devicesAndAssetsUHIARepository;
        private readonly IValidationEngine _validationEngine;
        public DeleteDevicesAndAssetsUHIACommandHandler(IDevicesAndAssetsUHIARepository devicesAndAssetsUHIARepository, IValidationEngine validationEngine)
        {
            _devicesAndAssetsUHIARepository = devicesAndAssetsUHIARepository;
            _validationEngine = validationEngine;
        }
        public async Task<bool> Handle(DeleteDevicesAndAssetsUHIACommand request, CancellationToken cancellationToken)
        {
            var DevicesAndAssetUHIA = await DevicesAndAssetsUHIA.Get(request.Id, _devicesAndAssetsUHIARepository);
            if (DevicesAndAssetUHIA is not null)
            {
                DevicesAndAssetUHIA.IsDeleted = true;
                DevicesAndAssetUHIA.IsDeletedBy = "tmp";
                for (int i = 0; i < DevicesAndAssetUHIA.ItemListPrices.Count; i++)
                {
                    var itemPrice = DevicesAndAssetUHIA.ItemListPrices.Where(x => x.Id == DevicesAndAssetUHIA.ItemListPrices[i].Id).FirstOrDefault();
                    if (itemPrice == null)
                    {
                        continue;
                    }

                    DevicesAndAssetUHIA.ItemListPrices[i].SetIsDeleted(true);
                    DevicesAndAssetUHIA.ItemListPrices[i].SetIsDeletedBy("tmp");

                    _validationEngine.Validate(DevicesAndAssetUHIA.ItemListPrices[i]);
                }

                return (await DevicesAndAssetUHIA.Delete(_devicesAndAssetsUHIARepository, _validationEngine));
            }
            else { throw new DataNotFoundException(); }
        }
    }
}
