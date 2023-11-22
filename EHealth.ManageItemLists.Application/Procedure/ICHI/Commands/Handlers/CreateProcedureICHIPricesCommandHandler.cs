using EHealth.ManageItemLists.Domain.Drugs.DrugsUHIA;
using EHealth.ManageItemLists.Domain.Procedures.ProceduresICHI;
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

namespace EHealth.ManageItemLists.Application.Procedure.ICHI.Commands.Handlers
{
    public class CreateProcedureICHIPricesCommandHandler : IRequestHandler<CreateProcedureICHIPricesCommand, Guid>
    {
        private readonly IProcedureICHIRepository _procedureICHIRepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;

        public CreateProcedureICHIPricesCommandHandler(IProcedureICHIRepository procedureICHIRepository,
        IValidationEngine validationEngine,
        IItemListPriceRepository itemListPriceRepository,
        IIdentityProvider identityProvider)
        {
            _procedureICHIRepository = procedureICHIRepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
        }
        public async Task<Guid> Handle(CreateProcedureICHIPricesCommand request, CancellationToken cancellationToken)
        {
            //validate model
            _validationEngine.Validate(request);

            var procedureICHI = await ProcedureICHI.Get(request.ProcedureICHIId, _procedureICHIRepository);
            var prices = request.ItemListPrices.Select(x => x.ToItemListPrice(_identityProvider.GetUserName(), _identityProvider.GetTenantId())).ToList();
            await procedureICHI.AddPrices(_procedureICHIRepository, _validationEngine, prices);

            return procedureICHI.Id;
        }
    }
}
