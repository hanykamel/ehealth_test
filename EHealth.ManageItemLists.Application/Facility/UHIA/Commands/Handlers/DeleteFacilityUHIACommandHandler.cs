using EHealth.ManageItemLists.Domain.Resource.UHIA;
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

namespace EHealth.ManageItemLists.Application.Facility.UHIA.Commands.Handlers
{
    public class DeleteFacilityUHIACommandHandler : IRequestHandler<DeleteFacilityUHIACommand, bool>
    {
        private readonly IFacilityUHIARepository _facilityUHIAsRepository;
        private readonly IIdentityProvider _identityProvider;
        private readonly IValidationEngine _validationEngine;

        public DeleteFacilityUHIACommandHandler(IFacilityUHIARepository facilityUHIAsRepository, IIdentityProvider identityProvider, IValidationEngine validationEngine)
        {
            _facilityUHIAsRepository = facilityUHIAsRepository;
            _identityProvider = identityProvider;
            _validationEngine = validationEngine;
        }

        public async Task<bool> Handle(DeleteFacilityUHIACommand request, CancellationToken cancellationToken)
        {
            var facilityUhia = await _facilityUHIAsRepository.Get(request.Id);

            if(facilityUhia == null)
            {
                throw new DataNotFoundException();
            }
            else
            {
                facilityUhia.SoftDelete(_identityProvider.GetUserName());
                return await facilityUhia.Delete(_facilityUHIAsRepository, _validationEngine);
            }
        }
    }
}
