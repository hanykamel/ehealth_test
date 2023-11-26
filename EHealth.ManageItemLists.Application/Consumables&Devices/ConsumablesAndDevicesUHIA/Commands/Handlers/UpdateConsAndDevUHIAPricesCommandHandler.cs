using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
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

namespace EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.Commands.Handlers
{
    public class UpdateConsAndDevUHIAPricesCommandHandler : IRequestHandler<UpdateConsAndDevUHIAPricesCommand, bool>
    {
        private readonly IConsumablesAndDevicesUHIARepository _consumablesAndDevicesUHIARepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;

        public UpdateConsAndDevUHIAPricesCommandHandler(IConsumablesAndDevicesUHIARepository consumablesAndDevicesUHIARepository,
        IValidationEngine validationEngine,
        IIdentityProvider identityProvider)
        {
            _consumablesAndDevicesUHIARepository = consumablesAndDevicesUHIARepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
        }
        public async Task<bool> Handle(UpdateConsAndDevUHIAPricesCommand request, CancellationToken cancellationToken)
        {
            //validate model
            _validationEngine.Validate(request);

            var consumablesAndDevicesUHIA = await Domain.ConsumablesAndDevices.ConsumablesAndDevicesUHIA.Get(request.ConsAndDevUHIAId, _consumablesAndDevicesUHIARepository);
            await Domain.ConsumablesAndDevices.ConsumablesAndDevicesUHIA.IsItemListBusy(_consumablesAndDevicesUHIARepository, consumablesAndDevicesUHIA.ItemListId);
            string userId = _identityProvider.GetUserName();
            string tenantId = _identityProvider.GetTenantId();

            // prepare model to update and soft delete Item Prices
            for (int i = 0; i < consumablesAndDevicesUHIA.ItemListPrices.Count; i++)
            {
                var itemPrice = request.ItemListPrices.Where(x => x.Id == consumablesAndDevicesUHIA.ItemListPrices[i].Id).FirstOrDefault();
                if (itemPrice == null)
                {
                    consumablesAndDevicesUHIA.ItemListPrices[i].SetIsDeleted(true);
                    consumablesAndDevicesUHIA.ItemListPrices[i].SetIsDeletedBy(userId);
                    consumablesAndDevicesUHIA.ItemListPrices[i].SetModifiedOn();
                }
                else
                {
                    consumablesAndDevicesUHIA.ItemListPrices[i].SetPrice(itemPrice.Price);
                    consumablesAndDevicesUHIA.ItemListPrices[i].SetEffectiveDateFrom(itemPrice.EffectiveDateFrom);
                    consumablesAndDevicesUHIA.ItemListPrices[i].SetEffectiveDateTo(itemPrice.EffectiveDateTo);
                    consumablesAndDevicesUHIA.ItemListPrices[i].SetModifiedOn();
                    consumablesAndDevicesUHIA.ItemListPrices[i].SetModifiedBy(userId);
                }
                _validationEngine.Validate(consumablesAndDevicesUHIA.ItemListPrices[i]);
            }

            // prepare model to add new Item Prices
            var addItemPrices = request.ItemListPrices.Where(x => x.Id == 0).ToList();
            foreach (var item in addItemPrices)
            {
                var itemListPrice = item.ToItemListPrice(userId, tenantId);
                _validationEngine.Validate(itemListPrice);
                consumablesAndDevicesUHIA.ItemListPrices.Add(itemListPrice);
            }

            // update data
            await consumablesAndDevicesUHIA.Update(_consumablesAndDevicesUHIARepository, _validationEngine);

            return true;
        }
    }
}
