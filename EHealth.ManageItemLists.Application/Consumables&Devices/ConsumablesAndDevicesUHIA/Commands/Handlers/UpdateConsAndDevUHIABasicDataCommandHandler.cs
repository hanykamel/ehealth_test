using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using EHealth.ManageItemLists.Domain.Shared.Identity;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using EHealth.ManageItemLists.Infrastructure.Repositories;
using MediatR;

namespace EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.Commands.Handlers
{
    public class UpdateConsAndDevUHIABasicDataCommandHandler : IRequestHandler<UpdateConsAndDevUHIABasicDataCommand, bool>
    {
        private readonly IConsumablesAndDevicesUHIARepository _consumablesAndDevicesUHIARepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;

        public UpdateConsAndDevUHIABasicDataCommandHandler(IConsumablesAndDevicesUHIARepository consumablesAndDevicesUHIARepository,
        IValidationEngine validationEngine,
        IIdentityProvider identityProvider)
        {
            _consumablesAndDevicesUHIARepository = consumablesAndDevicesUHIARepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
        }
        public async Task<bool> Handle(UpdateConsAndDevUHIABasicDataCommand request, CancellationToken cancellationToken)
        {
            //validate model
            _validationEngine.Validate(request);

            var consumablesAndDevicesUHIA = await Domain.ConsumablesAndDevices.ConsumablesAndDevicesUHIA.Get(request.Id, _consumablesAndDevicesUHIARepository);
            await Domain.ConsumablesAndDevices.ConsumablesAndDevicesUHIA.IsItemListBusy(_consumablesAndDevicesUHIARepository, consumablesAndDevicesUHIA.ItemListId);
            consumablesAndDevicesUHIA.SetEHealthCode(request.EHealthCode);
            consumablesAndDevicesUHIA.SetUHIAId(request.UHIAId);
            consumablesAndDevicesUHIA.SetShortDescAr(request.ShortDescAr);
            consumablesAndDevicesUHIA.SetShortDescEn(request.ShortDescEn);
            consumablesAndDevicesUHIA.SetServiceCategoryId(request.ServiceCategoryId);
            consumablesAndDevicesUHIA.SetServiceSubCategoryId(request.ServiceSubCategoryId);
            //serviceUHIA.SetItemListId(request.ItemListId);
            consumablesAndDevicesUHIA.SetDataEffectiveDateFrom(request.DataEffectiveDateFrom);
            consumablesAndDevicesUHIA.SetDataEffectiveDateTo(request.DataEffectiveDateTo);
            consumablesAndDevicesUHIA.SetUnitOfMeasureId(request.UnitOfMeasureId);
            consumablesAndDevicesUHIA.SetModifiedBy(_identityProvider.GetUserName());
            consumablesAndDevicesUHIA.SetModifiedOn();

            return (await consumablesAndDevicesUHIA.Update(_consumablesAndDevicesUHIARepository, _validationEngine));
        }
    }
}
