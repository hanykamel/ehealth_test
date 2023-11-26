using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using EHealth.ManageItemLists.Domain.DoctorFees.UHIA;
using EHealth.ManageItemLists.Domain.Shared.Identity;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using MediatR;

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
            await DoctorFeesUHIA.IsItemListBusy(_doctorFeesUHIARepository, request.ItemListId);
            var drFeesUHIA = request.ToDrFeesUHIA(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
            await drFeesUHIA.Create(_doctorFeesUHIARepository, _validationEngine);

            return drFeesUHIA.Id;
        }
    }
}
