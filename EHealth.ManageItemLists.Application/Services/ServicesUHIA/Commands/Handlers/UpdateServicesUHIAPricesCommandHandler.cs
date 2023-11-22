using EHealth.ManageItemLists.Domain.ItemListPricing;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using EHealth.ManageItemLists.Domain.Shared.Identity;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands.Handlers
{
    public class UpdateServicesUHIAPricesCommandHandler : IRequestHandler<UpdateServicesUHIAPricesCommand,bool>
    {
        private readonly IServiceUHIARepository _serviceUHIARepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;

        public UpdateServicesUHIAPricesCommandHandler(IServiceUHIARepository serviceUHIARepository,
        IValidationEngine validationEngine,
        IItemListPriceRepository itemListPriceRepository,
        IIdentityProvider identityProvider)
        {
            _serviceUHIARepository = serviceUHIARepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
        }
        public async Task<bool> Handle(UpdateServicesUHIAPricesCommand request, CancellationToken cancellationToken)
        {
            //validate model
            _validationEngine.Validate(request);

            var serviceUHIA = await ServiceUHIA.Get(request.ServiceUHIAId, _serviceUHIARepository);

            // Throw exception if item list busy
            await ServiceUHIA.IsItemListBusy(_serviceUHIARepository, serviceUHIA.ItemListId);

            var userId = _identityProvider.GetUserName();
            var tenantId = _identityProvider.GetTenantId();

            // prepare model to update and soft delete Item Prices
            for (int i = 0; i < serviceUHIA.ItemListPrices.Count; i++)
            {
                var itemPrice = request.ItemListPrices.Where(x => x.Id == serviceUHIA.ItemListPrices[i].Id).FirstOrDefault();
                if(itemPrice == null)
                {
                    serviceUHIA.ItemListPrices[i].SetIsDeleted(true);
                    serviceUHIA.ItemListPrices[i].SetIsDeletedBy(userId);
                }
                else
                {
                    serviceUHIA.ItemListPrices[i].SetPrice(itemPrice.Price);
                    serviceUHIA.ItemListPrices[i].SetEffectiveDateFrom(itemPrice.EffectiveDateFrom);
                    serviceUHIA.ItemListPrices[i].SetEffectiveDateTo(itemPrice.EffectiveDateTo);
                    serviceUHIA.ItemListPrices[i].SetModifiedBy(userId);
                }
                serviceUHIA.ItemListPrices[i].SetModifiedOn();
                _validationEngine.Validate(serviceUHIA.ItemListPrices[i]);
            }

            // prepare model to add new Item Prices
            var addItemPrices = request.ItemListPrices.Where(x => x.Id == 0).ToList();
            foreach (var item in addItemPrices)
            {
                var itemListPrice = item.ToItemListPrice(userId, tenantId);
                _validationEngine.Validate(itemListPrice);
                serviceUHIA.ItemListPrices.Add(itemListPrice);
            }

            // update data
            await serviceUHIA.Update(_serviceUHIARepository, _validationEngine);

            return true;
        }
    }
}
