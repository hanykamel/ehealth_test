using EHealth.ManageItemLists.Application.Services.ServicesUHIA.Commands;
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

namespace EHealth.ManageItemLists.Application.DoctorFees.UHIA.Commands.Handler
{
    public class CreateDoctorFeesUHIABasicDataCommandHandler : IRequestHandler<CreateDoctorFeesUHIABasicDataCommand, Guid>
    {
        private readonly IDoctorFeesUHIARepository _doctorFeesUHIARepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;
        public CreateDoctorFeesUHIABasicDataCommandHandler(IDoctorFeesUHIARepository doctorFeesUHIARepository, IValidationEngine validationEngine, IIdentityProvider identityProvider)
        {
            
            _doctorFeesUHIARepository = doctorFeesUHIARepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
        }
        public async Task<Guid> Handle(CreateDoctorFeesUHIABasicDataCommand request, CancellationToken cancellationToken)
        {

            var drFeesUHIA = request.ToDrFeesUHIA(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
            await drFeesUHIA.Create(_doctorFeesUHIARepository, _validationEngine);

            return drFeesUHIA.Id;
        }
    }
}
