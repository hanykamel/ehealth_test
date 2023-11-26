using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using EHealth.ManageItemLists.Infrastructure.Repositories;
using MediatR;

namespace EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.Commands.Handlers
{
    public class DeleteConsAndDevsUHIACommandHandler : IRequestHandler<DeleteConsAndDevsUHIACommand, bool>
    {
        private readonly IConsumablesAndDevicesUHIARepository _consumablesAndDevicesUHIARepository;
        private readonly IValidationEngine _validationEngine;
        public DeleteConsAndDevsUHIACommandHandler(IConsumablesAndDevicesUHIARepository consumablesAndDevicesUHIARepository, IValidationEngine validationEngine)
        {
            _consumablesAndDevicesUHIARepository = consumablesAndDevicesUHIARepository;
            _validationEngine = validationEngine;
        }
        public async Task<bool> Handle(DeleteConsAndDevsUHIACommand request, CancellationToken cancellationToken)
        {
            var ConsumablesAndDevicesUHIA = await Domain.ConsumablesAndDevices.ConsumablesAndDevicesUHIA.Get(request.Id, _consumablesAndDevicesUHIARepository);
            await  Domain.ConsumablesAndDevices.ConsumablesAndDevicesUHIA.IsItemListBusy(_consumablesAndDevicesUHIARepository, ConsumablesAndDevicesUHIA.ItemListId);

            if (ConsumablesAndDevicesUHIA is not null)
            {
                ConsumablesAndDevicesUHIA.IsDeleted = true;
                ConsumablesAndDevicesUHIA.IsDeletedBy = "tmp";
                for (int i = 0; i < ConsumablesAndDevicesUHIA.ItemListPrices.Count; i++)
                {
                    var itemPrice = ConsumablesAndDevicesUHIA.ItemListPrices.Where(x => x.Id == ConsumablesAndDevicesUHIA.ItemListPrices[i].Id).FirstOrDefault();
                    if (itemPrice == null)
                    {
                        continue;
                    }

                    ConsumablesAndDevicesUHIA.ItemListPrices[i].SetIsDeleted(true);
                    ConsumablesAndDevicesUHIA.ItemListPrices[i].SetIsDeletedBy("tmp");

                    _validationEngine.Validate(ConsumablesAndDevicesUHIA.ItemListPrices[i]);
                }

                return await ConsumablesAndDevicesUHIA.Delete(_consumablesAndDevicesUHIARepository, _validationEngine);
            }
            else { throw new DataNotFoundException(); }
        }
    }
}
