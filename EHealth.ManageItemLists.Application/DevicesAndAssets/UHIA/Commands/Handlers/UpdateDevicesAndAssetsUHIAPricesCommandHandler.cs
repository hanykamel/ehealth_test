using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
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

namespace EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.Commands.Handlers
{
    public class UpdateDevicesAndAssetsUHIAPricesCommandHandler : IRequestHandler<UpdateDevicesAndAssetsUHIAPricesCommand, bool>
    {
        private readonly IDevicesAndAssetsUHIARepository _devicesAndAssetsUHIARepository;
        private readonly IItemListPriceRepository _itemListPriceRepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;

        public UpdateDevicesAndAssetsUHIAPricesCommandHandler(IDevicesAndAssetsUHIARepository devicesAndAssetsUHIARepository,
        IValidationEngine validationEngine,
        IItemListPriceRepository itemListPriceRepository, IIdentityProvider identityProvider)
        {
            _devicesAndAssetsUHIARepository = devicesAndAssetsUHIARepository;
            _validationEngine = validationEngine;
            _itemListPriceRepository = itemListPriceRepository;
            _identityProvider = identityProvider;
        }
        public async Task<bool> Handle(UpdateDevicesAndAssetsUHIAPricesCommand request, CancellationToken cancellationToken)
        {
            //validate model
            _validationEngine.Validate(request);

            var devicesAndAssetsUHIA = await DevicesAndAssetsUHIA.Get(request.DevicesAndAssetsUHIAId, _devicesAndAssetsUHIARepository);
            await DevicesAndAssetsUHIA.IsItemListBusy(_devicesAndAssetsUHIARepository, devicesAndAssetsUHIA.ItemListId);
            // prepare model to update and soft delete Item Prices
            for (int i = 0; i < devicesAndAssetsUHIA.ItemListPrices.Count; i++)
            {
                var itemPrice = request.ItemListPrices.Where(x => x.Id == devicesAndAssetsUHIA.ItemListPrices[i].Id).FirstOrDefault();
                if (itemPrice == null)
                {
                    devicesAndAssetsUHIA.ItemListPrices[i].SetIsDeleted(true);
                    devicesAndAssetsUHIA.ItemListPrices[i].SetIsDeletedBy(_identityProvider.GetUserName());
                }
                else
                {
                    devicesAndAssetsUHIA.ItemListPrices[i].SetPrice(itemPrice.Price);
                    devicesAndAssetsUHIA.ItemListPrices[i].SetEffectiveDateFrom(itemPrice.EffectiveDateFrom);
                    devicesAndAssetsUHIA.ItemListPrices[i].SetEffectiveDateTo(itemPrice.EffectiveDateTo);
                    devicesAndAssetsUHIA.ItemListPrices[i].SetModifiedOn();
                    devicesAndAssetsUHIA.ItemListPrices[i].SetModifiedBy(_identityProvider.GetUserName());
                }
                _validationEngine.Validate(devicesAndAssetsUHIA.ItemListPrices[i]);
            }

            // prepare model to add new Item Prices
            var addItemPrices = request.ItemListPrices.Where(x => x.Id == 0).ToList();
            foreach (var item in addItemPrices)
            {
                var itemListPrice = item.ToItemListPrice(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
                _validationEngine.Validate(itemListPrice);
                devicesAndAssetsUHIA.ItemListPrices.Add(itemListPrice);
            }

            // update data
            await devicesAndAssetsUHIA.Update(_devicesAndAssetsUHIARepository, _validationEngine, _identityProvider.GetUserName());

            return true;
        }
    }
}
