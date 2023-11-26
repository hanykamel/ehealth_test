using EHealth.ManageItemLists.Application.DevicesAndAssets.UHIA.Commands;
using EHealth.ManageItemLists.Domain.Procedures.ProceduresICHI;
using EHealth.ManageItemLists.Domain.Resource.UHIA;
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
    public class CreateResourceUHIABasicDataCommandHandler : IRequestHandler<CreateResourceUHIABasicDataCommand, Guid>
    {
        private readonly IResourceUHIARepository _resourceUHIARepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;

        public CreateResourceUHIABasicDataCommandHandler(IResourceUHIARepository resourceUHIARepository,
        IValidationEngine validationEngine, IIdentityProvider identityProvider)
        {
            _resourceUHIARepository = resourceUHIARepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
        }
        public async Task<Guid> Handle(CreateResourceUHIABasicDataCommand request, CancellationToken cancellationToken)
        {
            await ResourceUHIA.IsItemListBusy(_resourceUHIARepository, request.ItemListId);
            var resourceUHIA = request.ToResourceUHIA(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
            await resourceUHIA.Create(_resourceUHIARepository, _validationEngine);

            return resourceUHIA.Id;
        }
    }
}
