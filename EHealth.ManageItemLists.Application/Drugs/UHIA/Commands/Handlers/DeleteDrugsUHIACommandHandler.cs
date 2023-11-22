using EHealth.ManageItemLists.Application.Resource.UHIA.Commands;
using EHealth.ManageItemLists.DataAccess.Migrations;
using EHealth.ManageItemLists.Domain.Drugs.DrugsUHIA;
using EHealth.ManageItemLists.Domain.Resource.UHIA;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
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

namespace EHealth.ManageItemLists.Application.Drugs.UHIA.Commands.Handlers
{
    public class DeleteDrugsUHIACommandHandler : IRequestHandler<DeleteDrugsUHIACommand, bool>
    {
        private readonly IDrugsUHIARepository _drugsUHIARepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;

        public DeleteDrugsUHIACommandHandler(IDrugsUHIARepository drugsUHIARepository, IValidationEngine validationEngine, IIdentityProvider identityProvider)
        {
            _drugsUHIARepository = drugsUHIARepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
        }
        public async Task<bool> Handle(DeleteDrugsUHIACommand request, CancellationToken cancellationToken)
        {
            var drugsUHIA = await DrugUHIA.Get(request.Id, _drugsUHIARepository);
            if (drugsUHIA is not null)
            {
                drugsUHIA.SoftDelete(_identityProvider.GetUserName());

                for (int i = 0; i < drugsUHIA.DrugPrices.Count; i++)
                {
                    var itemPrice = drugsUHIA.DrugPrices.Where(x => x.Id == drugsUHIA.DrugPrices[i].Id).FirstOrDefault();
                    if (itemPrice == null)
                    {
                        continue;
                    }

                    drugsUHIA.DrugPrices[i].SoftDelete(_identityProvider.GetUserName());

                    _validationEngine.Validate(drugsUHIA.DrugPrices[i]);
                }

                return (await drugsUHIA.Delete(_drugsUHIARepository, _validationEngine));
            }
            else { throw new DataNotFoundException(); }

        }


    }
}
