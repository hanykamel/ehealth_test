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
    public class CreateServicesUHIABasicDataCommandHandler: IRequestHandler<CreateServicesUHIABasicDataCommand, Guid>
    {
        private readonly IServiceUHIARepository _serviceUHIARepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;

        public CreateServicesUHIABasicDataCommandHandler(IServiceUHIARepository serviceUHIARepository,
        IValidationEngine validationEngine,
        IIdentityProvider identityProvider)
        {
            _serviceUHIARepository = serviceUHIARepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
        }
        //add documentation to this function

       
        public async Task<Guid> Handle(CreateServicesUHIABasicDataCommand request, CancellationToken cancellationToken)
        {
            // Throw exception if item list busy
            await ServiceUHIA.IsItemListBusy(_serviceUHIARepository, request.ItemListId);
            //call request to create serviceUHIA and pass the repository and validation engine
            var serviceUHIA = request.ToServiceUHIA(_identityProvider.GetUserName(), _identityProvider.GetTenantId());

            
            await serviceUHIA.Create(_serviceUHIARepository, _validationEngine);

            return serviceUHIA.Id;
        }
    }
}
