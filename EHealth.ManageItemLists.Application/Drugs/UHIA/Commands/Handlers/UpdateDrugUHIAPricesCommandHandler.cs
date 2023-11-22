using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands;
using EHealth.ManageItemLists.DataAccess.Migrations;
using EHealth.ManageItemLists.Domain.Drugs.DrugsUHIA;
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

namespace EHealth.ManageItemLists.Application.Drugs.UHIA.Commands.Handlers
{
    public class UpdateDrugUHIAPricesCommandHandler : IRequestHandler<UpdateDrugUHIAPricesCommand, bool>
    {
        private readonly IDrugsUHIARepository _drugsUHIARepository;
        private readonly IItemListPriceRepository _itemListPriceRepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;

        public UpdateDrugUHIAPricesCommandHandler(IDrugsUHIARepository drugsUHIARepository,
        IValidationEngine validationEngine,
        IItemListPriceRepository itemListPriceRepository,
        IIdentityProvider identityProvider)
        {
            _drugsUHIARepository = drugsUHIARepository;
            _validationEngine = validationEngine;
            _itemListPriceRepository = itemListPriceRepository;
            _identityProvider = identityProvider;
        }
        public async Task<bool> Handle(UpdateDrugUHIAPricesCommand request, CancellationToken cancellationToken)
        {
            //validate model
            _validationEngine.Validate(request);

            var drugUHIA = await DrugUHIA.Get(request.Id, _drugsUHIARepository);

            //get edited items
            var editedItemsIds = request.drugPrices.Where(p => p.Id != 0).Select(p => p.Id);
            //get deleted items
            var deletedItems = drugUHIA.DrugPrices.Where(p => !editedItemsIds.Contains(p.Id)).ToList();
            //delete deleted items
            drugUHIA.DeleteDrugPrices(deletedItems, _identityProvider.GetUserName());
            //edit edited items
            var editedItems = drugUHIA.DrugPrices.Where(p => editedItemsIds.Contains(p.Id));
            foreach(var item in editedItems)
            {
                var updatedItem = request.drugPrices.FirstOrDefault(i => i.Id == item.Id).ToDrugPrice(item.CreatedBy, item.TenantId);
                item.Update(updatedItem, _identityProvider.GetUserName());
                _validationEngine.Validate(item);
            }

            //add new items
            var newItems = request.drugPrices.Where(p => p.Id == 0).Select(i => i.ToDrugPrice(_identityProvider.GetUserName(), _identityProvider.GetTenantId()));
            foreach(var item in newItems)
            {
                _validationEngine.Validate(item);
                drugUHIA.DrugPrices.Add(item);
            }
            return await drugUHIA.Update(_drugsUHIARepository, _validationEngine, _identityProvider.GetUserName());
        }
    }
}
