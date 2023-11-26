using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands;
using EHealth.ManageItemLists.Domain.Drugs.DrugsUHIA;
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
    public class CreateDrugUHIABasicDataCommandHandler : IRequestHandler<CreateDrugUHIABasicDataCommand, Guid>
    {
        private readonly IDrugsUHIARepository _drugsUHIARepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;

        public CreateDrugUHIABasicDataCommandHandler(IDrugsUHIARepository drugsUHIARepository,
        IValidationEngine validationEngine, IIdentityProvider identityProvider)
        {
            _drugsUHIARepository = drugsUHIARepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
        }
        public async Task<Guid> Handle(CreateDrugUHIABasicDataCommand request, CancellationToken cancellationToken)
        {
            await DrugUHIA.IsItemListBusy(_drugsUHIARepository, request.ItemListId);
            var drugUHIA = request.ToDrugUHIA(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
            await drugUHIA.Create(_drugsUHIARepository, _validationEngine);
            return drugUHIA.Id;
        }
    }
}
