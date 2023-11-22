using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands;
using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using EHealth.ManageItemLists.Domain.ItemListPricing;
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

namespace EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.Commands.Handlers
{
    public class CreateDevicesAndAssetsUHIAPricesCommandHandler : IRequestHandler<CreateDevicesAndAssetsUHIAPricesCommand, Guid>
    {
        private readonly IDevicesAndAssetsUHIARepository _devicesAndAssetsUHIARepository;
        private readonly IItemListPriceRepository _itemListPriceRepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;

        public CreateDevicesAndAssetsUHIAPricesCommandHandler(IDevicesAndAssetsUHIARepository devicesAndAssetsUHIARepository,
        IValidationEngine validationEngine,
        IItemListPriceRepository itemListPriceRepository, IIdentityProvider identityProvider)
        {
            _devicesAndAssetsUHIARepository = devicesAndAssetsUHIARepository;
            _validationEngine = validationEngine;
            _itemListPriceRepository = itemListPriceRepository;
            _identityProvider = identityProvider;
        }
        public async Task<Guid> Handle(CreateDevicesAndAssetsUHIAPricesCommand request, CancellationToken cancellationToken)
        {
            //validate model
            _validationEngine.Validate(request);

            var devicesAndAssetsUHIA = await DevicesAndAssetsUHIA.Get(request.DevicesAndAssetsUHIAId, _devicesAndAssetsUHIARepository);

            foreach (var item in request.ItemListPrices)
            {
                var itemListPrice = item.ToItemListPrice(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
                _validationEngine.Validate(itemListPrice);
                devicesAndAssetsUHIA.ItemListPrices.Add(itemListPrice);
            }
            await devicesAndAssetsUHIA.Update(_devicesAndAssetsUHIARepository, _validationEngine, _identityProvider.GetUserName());

            return devicesAndAssetsUHIA.Id;
        }
    }
}
