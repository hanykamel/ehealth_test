using EHealth.ManageItemLists.DataAccess.Migrations;
using EHealth.ManageItemLists.Domain.Drugs.DrugsUHIA;
using EHealth.ManageItemLists.Domain.Procedures.ProceduresICHI;
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

namespace EHealth.ManageItemLists.Application.Procedure.ICHI.Commands.Handlers
{
    public class UpdateProcedureICHIPricesCommandHandler : IRequestHandler<UpdateProcedureICHIPricesCommand, bool>
    {
        private readonly IProcedureICHIRepository _procedureICHIRepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;

        public UpdateProcedureICHIPricesCommandHandler(IProcedureICHIRepository procedureICHIRepository,
        IValidationEngine validationEngine,
        IItemListPriceRepository itemListPriceRepository,
        IIdentityProvider identityProvider)
        {
            _procedureICHIRepository = procedureICHIRepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
        }
        public async Task<bool> Handle(UpdateProcedureICHIPricesCommand request, CancellationToken cancellationToken)
        {
            //validate model
            _validationEngine.Validate(request);

            var procedureICHI = await ProcedureICHI.Get(request.ProcedureICHIId, _procedureICHIRepository);
            await ProcedureICHI.IsItemListBusy(_procedureICHIRepository, procedureICHI.ItemListId);
            var userId = _identityProvider.GetUserName();
            var tenantId = _identityProvider.GetTenantId();


            //get edited items
            var editedItemsIds = request.ItemListPrices.Where(p => p.Id != 0).Select(p => p.Id);
            //get deleted items
            var deletedItems = procedureICHI.ItemListPrices.Where(p => !editedItemsIds.Contains(p.Id)).ToList();
            //delete deleted items
            procedureICHI.DeletePrices(deletedItems, userId);
            //edit edited items
            var editedItems = procedureICHI.ItemListPrices.Where(p => editedItemsIds.Contains(p.Id));
            foreach (var item in editedItems)
            {
                var updatedItem = request.ItemListPrices.FirstOrDefault(i => i.Id == item.Id).ToItemListPrice(item.CreatedBy, item.TenantId);
                item.Update(updatedItem, _identityProvider.GetUserName());
                _validationEngine.Validate(item);
            }

            //add new items
            var newItems = request.ItemListPrices.Where(p => p.Id == 0).Select(i => i.ToItemListPrice(_identityProvider.GetUserName(), _identityProvider.GetTenantId()));
            foreach (var item in newItems)
            {
                _validationEngine.Validate(item);
                procedureICHI.ItemListPrices.Add(item);
            }
            return await procedureICHI.Update(_procedureICHIRepository, _validationEngine, _identityProvider.GetUserName());
        }
    }
}
