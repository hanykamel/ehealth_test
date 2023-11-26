using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands;
using EHealth.ManageItemLists.Domain.Resource.UHIA;
using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Identity;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Resource.UHIA.Commands.Handlers
{
    public class DeleteResourceUHIACommandHandler : IRequestHandler<DeleteResourceUHIACommand, bool>
    {
        private readonly IResourceUHIARepository _resourceUHIARepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;

        public DeleteResourceUHIACommandHandler(IResourceUHIARepository resourceUHIARepository, IValidationEngine validationEngine, IIdentityProvider identityProvider)
        {
            _resourceUHIARepository = resourceUHIARepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
        }
        public async Task<bool> Handle(DeleteResourceUHIACommand request, CancellationToken cancellationToken)
        {
            var resourceUHIA = await ResourceUHIA.Get(request.Id, _resourceUHIARepository);
            await ResourceUHIA.IsItemListBusy(_resourceUHIARepository, resourceUHIA.ItemListId);
            if (resourceUHIA is not null)
            {
                resourceUHIA.SoftDelete(_identityProvider.GetUserName());

                for (int i = 0; i < resourceUHIA.ItemListPrices.Count; i++)
                {
                    var itemPrice = resourceUHIA.ItemListPrices.Where(x => x.Id == resourceUHIA.ItemListPrices[i].Id).FirstOrDefault();
                    if (itemPrice == null)
                    {
                        continue;
                    }

                    resourceUHIA.ItemListPrices[i].SoftDelete(_identityProvider.GetUserName());

                    _validationEngine.Validate(resourceUHIA.ItemListPrices[i]);
                }

                return (await resourceUHIA.Delete(_resourceUHIARepository, _validationEngine));
            }
            else { throw new DataNotFoundException(); }

        }


    }
}
