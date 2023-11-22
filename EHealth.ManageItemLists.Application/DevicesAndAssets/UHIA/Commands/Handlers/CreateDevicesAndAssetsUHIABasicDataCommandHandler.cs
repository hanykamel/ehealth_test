using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands;
using EHealth.ManageItemLists.Domain.Shared.Identity;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.Commands.Handlers
{
    public class CreateDevicesAndAssetsUHIABasicDataCommandHandler : IRequestHandler<CreateDevicesAndAssetsUHIABasicDataCommand, Guid>
    {
        private readonly IDevicesAndAssetsUHIARepository _devicesAndAssetsUHIARepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;

        public CreateDevicesAndAssetsUHIABasicDataCommandHandler(IDevicesAndAssetsUHIARepository devicesAndAssetsUHIARepository,
        IValidationEngine validationEngine, IIdentityProvider identityProvider)
        {
            _devicesAndAssetsUHIARepository = devicesAndAssetsUHIARepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
        }
        public async Task<Guid> Handle(CreateDevicesAndAssetsUHIABasicDataCommand request, CancellationToken cancellationToken)
        {

            var devicesAndAssetsUHIA = request.ToDevicesAndAssetsUHIA(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
            await devicesAndAssetsUHIA.Create(_devicesAndAssetsUHIARepository, _validationEngine);

            return devicesAndAssetsUHIA.Id;
        }
    }
}
