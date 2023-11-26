using EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.Commands;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands;
using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
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
    public class UpdateDevicesAndAssetsUHIABasicDataCommandHandler : IRequestHandler<UpdateDevicesAndAssetsUHIABasicDataCommand, bool>
    {
        private readonly IDevicesAndAssetsUHIARepository _devicesAndAssetsUHIARepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;

        public UpdateDevicesAndAssetsUHIABasicDataCommandHandler(IDevicesAndAssetsUHIARepository devicesAndAssetsUHIARepository,
        IValidationEngine validationEngine, IIdentityProvider identityProvider)
        {
            _devicesAndAssetsUHIARepository = devicesAndAssetsUHIARepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
        }
        public async Task<bool> Handle(UpdateDevicesAndAssetsUHIABasicDataCommand request, CancellationToken cancellationToken)
        {
            //validate model
            _validationEngine.Validate(request);

            var devicesAndAssetsUHIA = await DevicesAndAssetsUHIA.Get(request.Id, _devicesAndAssetsUHIARepository);
            await DevicesAndAssetsUHIA.IsItemListBusy(_devicesAndAssetsUHIARepository, devicesAndAssetsUHIA.ItemListId);
            devicesAndAssetsUHIA.SetEHealthCode(request.EHealthCode);
            devicesAndAssetsUHIA.SetUnitRoomId(request.UnitRoomId);
            devicesAndAssetsUHIA.SetDescriptorAr(request.DescriptorAr);
            devicesAndAssetsUHIA.SetDescriptorEn(request.DescriptorEn);
            devicesAndAssetsUHIA.SetCategoryId(request.CategoryId);
            devicesAndAssetsUHIA.SetSubCategoryId(request.SubCategoryId);
            devicesAndAssetsUHIA.SetDataEffectiveDateFrom(request.DataEffectiveDateFrom);
            devicesAndAssetsUHIA.SetDataEffectiveDateTo(request.DataEffectiveDateTo);

            return await devicesAndAssetsUHIA.Update(_devicesAndAssetsUHIARepository, _validationEngine, _identityProvider.GetUserName());
        }
    }
}
