using EHealth.ManageItemLists.Application.Drugs.UHIA.Commands;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands;
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
    public class CreateResourceUHIAPricesCommandHandler : IRequestHandler<CreateResourceUHIAPricesCommand, bool>
    {
        private readonly IResourceUHIARepository _resourceUHIARepository;
        private readonly IResourceItemPriceRepository _resourceItemPriceRepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;

        public CreateResourceUHIAPricesCommandHandler(IResourceUHIARepository resourceUHIARepository,
        IValidationEngine validationEngine,
        IResourceItemPriceRepository resourceItemPriceRepository, IIdentityProvider identityProvider)
        {
            _resourceUHIARepository = resourceUHIARepository;
            _validationEngine = validationEngine;
            _resourceItemPriceRepository = resourceItemPriceRepository;
            _identityProvider = identityProvider;
        }
        public async Task<bool> Handle(CreateResourceUHIAPricesCommand request, CancellationToken cancellationToken)
        {
            //validate model 
            _validationEngine.Validate(request);
            var resourceUHIA = await ResourceUHIA.Get(request.ResourceUHIAId, _resourceUHIARepository);
            var resourcePrices = request.ResourceItemPrices.Select(p => p.ToResourceItemPrice(_identityProvider.GetUserName(), _identityProvider.GetTenantId())).ToList();
            await resourceUHIA.AddResourcePrices(_resourceUHIARepository, _validationEngine, resourcePrices);
            return true;

        }
    }
}
