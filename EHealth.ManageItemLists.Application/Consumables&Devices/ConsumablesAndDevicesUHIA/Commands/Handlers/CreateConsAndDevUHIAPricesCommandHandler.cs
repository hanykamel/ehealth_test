using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands;
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

namespace EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.Commands.Handlers
{
    public class CreateConsAndDevUHIAPricesCommandHandler : IRequestHandler<CreateConsAndDevUHIAPricesCommand, Guid>
    {
        private readonly IConsumablesAndDevicesUHIARepository _consumablesAndDevicesUHIARepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;

        public CreateConsAndDevUHIAPricesCommandHandler(IConsumablesAndDevicesUHIARepository consumablesAndDevicesUHIARepository,
        IValidationEngine validationEngine,
        IIdentityProvider identityProvider)
        {
            _consumablesAndDevicesUHIARepository = consumablesAndDevicesUHIARepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
        }
        public async Task<Guid> Handle(CreateConsAndDevUHIAPricesCommand request, CancellationToken cancellationToken)
        {
            //validate model
            _validationEngine.Validate(request);

            var consumablesAndDevicesUHIA = await Domain.ConsumablesAndDevices.ConsumablesAndDevicesUHIA.Get(request.ConsumablesAndDevicesUHIAId, _consumablesAndDevicesUHIARepository);

            foreach (var item in request.ItemListPrices)
            {
                var itemListPrice = item.ToItemListPrice(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
                _validationEngine.Validate(itemListPrice);
                consumablesAndDevicesUHIA.ItemListPrices.Add(itemListPrice);
            }

            await consumablesAndDevicesUHIA.Update(_consumablesAndDevicesUHIARepository, _validationEngine);

            return consumablesAndDevicesUHIA.Id;
        }
    }
}
