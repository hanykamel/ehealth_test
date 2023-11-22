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
    public class CreateServicesUHIAPricesCommandHandler: IRequestHandler<CreateServicesUHIAPricesCommand, Guid>
    {
        private readonly IServiceUHIARepository _serviceUHIARepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;

        public CreateServicesUHIAPricesCommandHandler(IServiceUHIARepository serviceUHIARepository,
        IValidationEngine validationEngine,
        IItemListPriceRepository itemListPriceRepository,
        IIdentityProvider identityProvider)
        {
            _serviceUHIARepository = serviceUHIARepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
        }
        public async Task<Guid> Handle(CreateServicesUHIAPricesCommand request, CancellationToken cancellationToken)
        {
            //validate model
            _validationEngine.Validate(request);
          
            var serviceUHIA = await ServiceUHIA.Get(request.ServiceUHIAId, _serviceUHIARepository);

            // Throw exception if item list busy
            await ServiceUHIA.IsItemListBusy(_serviceUHIARepository, serviceUHIA.ItemListId);

            foreach (var item in request.ItemListPrices)
            {
                var itemListPrice = item.ToItemListPrice(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
                _validationEngine.Validate(itemListPrice);
                serviceUHIA.ItemListPrices.Add(itemListPrice);
            }
            await serviceUHIA.Update(_serviceUHIARepository, _validationEngine);

            return serviceUHIA.Id;
        }
    }
}
