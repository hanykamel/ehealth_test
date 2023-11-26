using EHealth.ManageItemLists.Application.Drugs.UHIA.DTOs;
using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands;
using EHealth.ManageItemLists.DataAccess.Migrations;
using EHealth.ManageItemLists.Domain.DoctorFees.UHIA;
using EHealth.ManageItemLists.Domain.Drugs.DrugsUHIA;
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

namespace EHealth.ManageItemLists.Application.Drugs.UHIA.Commands.Handlers
{
    public class CreateDrugsUHIAPricesCommandHandler : IRequestHandler<CreateDrugsUHIAPricesCommand, bool>
    {
        private readonly IDrugsUHIARepository _drugsUHIARepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;

        public CreateDrugsUHIAPricesCommandHandler(IDrugsUHIARepository drugsUHIARepository,
        IValidationEngine validationEngine, IIdentityProvider identityProvider)
        {
            _drugsUHIARepository = drugsUHIARepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
        }
        public async Task<bool> Handle(CreateDrugsUHIAPricesCommand request, CancellationToken cancellationToken)
        {
            //validate model 
            _validationEngine.Validate(request);
            var drugUHIA = await DrugUHIA.Get(request.Id, _drugsUHIARepository);
            await DrugUHIA.IsItemListBusy(_drugsUHIARepository, drugUHIA.ItemListId);
            var drugPrices = request.ItemListPrices.Select(p => p.ToDrugPrice(_identityProvider.GetUserName(),_identityProvider.GetTenantId())).ToList();
            await drugUHIA.AddDrugPrices(_drugsUHIARepository, _validationEngine, drugPrices);
            return true;

        }
    }
}
