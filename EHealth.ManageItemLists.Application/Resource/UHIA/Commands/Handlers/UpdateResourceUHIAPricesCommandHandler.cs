
using EHealth.ManageItemLists.Domain.Drugs.DrugsUHIA;
using EHealth.ManageItemLists.Domain.Resource.UHIA;
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

namespace EHealth.ManageItemLists.Application.Resource.UHIA.Commands.Handlers
{
    public class UpdateResourceUHIAPricesCommandHandler : IRequestHandler<UpdateResourceUHIAPricesCommand, bool>
    {
        private readonly IResourceUHIARepository _resourcesUHIARepository;
        private readonly IResourceItemPriceRepository _itemListPriceRepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;

        public UpdateResourceUHIAPricesCommandHandler(IResourceUHIARepository resourcesUHIARepository,
        IValidationEngine validationEngine,
        IResourceItemPriceRepository itemListPriceRepository,
        IIdentityProvider identityProvider)
        {
            _resourcesUHIARepository = resourcesUHIARepository;
            _validationEngine = validationEngine;
            _itemListPriceRepository = itemListPriceRepository;
            _identityProvider = identityProvider;
        }
        public async Task<bool> Handle(UpdateResourceUHIAPricesCommand request, CancellationToken cancellationToken)
        {
            //validate model
            _validationEngine.Validate(request);

            var resourceUHIA = await ResourceUHIA.Get(request.ResourceUHIAId, _resourcesUHIARepository);

            //get edited items
            var editedItemsIds = request.ResourceItemPrices.Where(p => p.Id != 0).Select(p => p.Id);
            //get deleted items
            var deletedItems = resourceUHIA.ItemListPrices.Where(p => !editedItemsIds.Contains(p.Id)).ToList();
            //delete deleted items
            resourceUHIA.DeleteResourcePrices(deletedItems, _identityProvider.GetUserName());

            //edit edited items
            var editedItems = resourceUHIA.ItemListPrices.Where(p => editedItemsIds.Contains(p.Id));
            foreach (var item in editedItems)
            {
                var updatedItem = request.ResourceItemPrices.FirstOrDefault(i => i.Id == item.Id).ToResourceItemPrice(item.CreatedBy, item.TenantId);
                item.Update(updatedItem, _identityProvider.GetUserName());
                _validationEngine.Validate(item);
            }

            //add new items
            var newItems = request.ResourceItemPrices.Where(p => p.Id == 0).Select(i => i.ToResourceItemPrice(_identityProvider.GetUserName(), _identityProvider.GetTenantId()));
            foreach (var item in newItems)
            {
                _validationEngine.Validate(item);
                resourceUHIA.ItemListPrices.Add(item);
            }
            return await resourceUHIA.Update(_resourcesUHIARepository, _validationEngine, _identityProvider.GetUserName());
        }
    }
}
