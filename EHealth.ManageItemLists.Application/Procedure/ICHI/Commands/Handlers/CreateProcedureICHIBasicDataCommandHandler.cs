using EHealth.ManageItemLists.Domain.Shared.Identity;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
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

            var procedureICHI = request.ToProcedureICHI(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
            await procedureICHI.Create(_procedureICHIRepository, _validationEngine);

            return procedureICHI.Id;
        }
    }
}
