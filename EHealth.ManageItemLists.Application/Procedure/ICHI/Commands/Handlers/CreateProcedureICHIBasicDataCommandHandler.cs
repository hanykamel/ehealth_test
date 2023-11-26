using EHealth.ManageItemLists.Domain.Facility.UHIA;
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
    public class CreateProcedureICHIBasicDataCommandHandler : IRequestHandler<CreateProcedureICHIBasicDataCommand, Guid>
    {
        private readonly IProcedureICHIRepository _procedureICHIRepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;

        public CreateProcedureICHIBasicDataCommandHandler(IProcedureICHIRepository procedureICHIRepository,
        IValidationEngine validationEngine,
        IIdentityProvider identityProvider)
        {
            _procedureICHIRepository = procedureICHIRepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
        }
        public async Task<Guid> Handle(CreateProcedureICHIBasicDataCommand request, CancellationToken cancellationToken)
        {
            await ProcedureICHI.IsItemListBusy(_procedureICHIRepository, request.ItemListId);
            var procedureICHI = request.ToProcedureICHI(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
            await procedureICHI.Create(_procedureICHIRepository, _validationEngine);

            return procedureICHI.Id;
        }
    }
}
