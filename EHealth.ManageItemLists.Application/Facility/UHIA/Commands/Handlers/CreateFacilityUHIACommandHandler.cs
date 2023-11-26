using EHealth.ManageItemLists.Application.Facility.UHIA.DTOs;
using EHealth.ManageItemLists.Application.ItemLists.Commands;
using EHealth.ManageItemLists.Application.ItemLists.DTOs;
using EHealth.ManageItemLists.DataAccess.Migrations;
using EHealth.ManageItemLists.Domain.Drugs.DrugsUHIA;
using EHealth.ManageItemLists.Domain.Facility.UHIA;
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

namespace EHealth.ManageItemLists.Application.Facility.UHIA.Commands.Handlers
{
    public class CreateFacilityUHIACommandHandler : IRequestHandler<CreateFacilityUHIACommand, Guid>
    {
        private readonly IValidationEngine _validationEngine;
        private readonly IMediator _mediator;
        private readonly IFacilityUHIARepository _facilityUHIAsRepository;
        private readonly IIdentityProvider _identityProvider;

        public CreateFacilityUHIACommandHandler(IValidationEngine validationEngine, IMediator mediator, IFacilityUHIARepository facilityUHIAsRepository, IIdentityProvider identityProvider)
        {
            _validationEngine = validationEngine;
            _mediator = mediator;
            _facilityUHIAsRepository = facilityUHIAsRepository;
            _identityProvider = identityProvider;
        }

        public async Task<Guid> Handle(CreateFacilityUHIACommand request, CancellationToken cancellationToken)
        {
            await FacilityUHIA.IsItemListBusy(_facilityUHIAsRepository, request.CreateFacilityUHIADto.ItemListId);
            var facilityUhia = request.CreateFacilityUHIADto.ToFacilityUHIA(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
            await facilityUhia.Create(_facilityUHIAsRepository, _validationEngine);

            return facilityUhia.Id;
        }
    }
}
