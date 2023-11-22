using EHealth.ManageItemLists.Domain.DoctorFees.UHIA;
using EHealth.ManageItemLists.Domain.Shared.Identity;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace EHealth.ManageItemLists.Application.DoctorFees.UHIA.Commands.Handler
{
    public class CreateDoctorFeesUHIAPricesCommandHandler : IRequestHandler<CreateDoctorFeesUHIAPricesCommand, Guid>
    {
        private readonly IDoctorFeesUHIARepository _doctorFeesUHIARepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;
        public CreateDoctorFeesUHIAPricesCommandHandler(IDoctorFeesUHIARepository doctorFeesUHIARepository, IValidationEngine validationEngine, IIdentityProvider identityProvider)
        {
            _doctorFeesUHIARepository = doctorFeesUHIARepository;
            _validationEngine = validationEngine;   
            _identityProvider = identityProvider;
        }
        public async Task<Guid> Handle(CreateDoctorFeesUHIAPricesCommand request, CancellationToken cancellationToken)
        {
            //validate model
            _validationEngine.Validate(request);

            var drFeesUHIA = await DoctorFeesUHIA.Get(request.DoctorFeesUHIAId, _doctorFeesUHIARepository);

            foreach (var item in request.ItemListPrices)
            {
                var itemListPrice = item.ToDrFeesItemPrice(_identityProvider.GetUserName(), _identityProvider.GetTenantId());
                _validationEngine.Validate(itemListPrice);
                drFeesUHIA.ItemListPrices.Add(itemListPrice);
            }
            await drFeesUHIA.Update(_doctorFeesUHIARepository, _validationEngine,_identityProvider.GetUserName());

            return drFeesUHIA.Id;
        }
    }
}
