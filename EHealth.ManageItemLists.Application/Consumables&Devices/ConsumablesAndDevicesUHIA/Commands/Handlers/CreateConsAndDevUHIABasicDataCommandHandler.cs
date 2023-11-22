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

namespace EHealth.ManageItemLists.Application.Consumables_Devices.ConsumablesAndDevicesUHIA.Commands.Handlers
{
    internal class CreateConsAndDevUHIABasicDataCommandHandler : IRequestHandler<CreateConsAndDevUHIABasicDataCommand, Guid>
    {
        private readonly IConsumablesAndDevicesUHIARepository _consumablesAndDevicesUHIARepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;

        public CreateConsAndDevUHIABasicDataCommandHandler(IConsumablesAndDevicesUHIARepository consumablesAndDevicesUHIARepository,
        IValidationEngine validationEngine,
        IIdentityProvider identityProvider)
        {
            _consumablesAndDevicesUHIARepository = consumablesAndDevicesUHIARepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
        }
        public async Task<Guid> Handle(CreateConsAndDevUHIABasicDataCommand request, CancellationToken cancellationToken)
        {

            var consumablesAndDevicesUHIA = request.ToConsumablesAndDevicesUHIA(_identityProvider.GetUserName(), _identityProvider.GetTenantId());


            return (await consumablesAndDevicesUHIA.Create(_consumablesAndDevicesUHIARepository, _validationEngine));
        }
    }
}