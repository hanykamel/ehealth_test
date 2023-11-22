using EHealth.ManageItemLists.Domain.DoctorFees.UHIA;
using EHealth.ManageItemLists.Domain.Shared.Identity;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using MediatR;

namespace EHealth.ManageItemLists.Application.DoctorFees.UHIA.Commands.Handler
{
    public class UpdateDoctorFeesUHIABasicDataCommandHandler : IRequestHandler<UpdateDoctorFeesUHIABasicDataCommand, bool>
    {
        private readonly IDoctorFeesUHIARepository _doctorFeesUHIARepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;
        public UpdateDoctorFeesUHIABasicDataCommandHandler(IDoctorFeesUHIARepository doctorFeesUHIARepository, IValidationEngine validationEngine
            , IIdentityProvider identityProvider )
        {
            _doctorFeesUHIARepository = doctorFeesUHIARepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
        }
        public async Task<bool> Handle(UpdateDoctorFeesUHIABasicDataCommand request, CancellationToken cancellationToken)
        {
            //validate model
            _validationEngine.Validate(request);

            var doctorFeesUHIA = await DoctorFeesUHIA.Get(request.Id, _doctorFeesUHIARepository);
            doctorFeesUHIA.SetEHealthCode(request.EHealthCode);
            doctorFeesUHIA.SetDescriptorAr(request.DescriptorAr);
            doctorFeesUHIA.SetDescriptorEn(request.DescriptorEn);
            doctorFeesUHIA.SetPackageComplexityClassificationId(request.PackageCompexityClassificationId);
            doctorFeesUHIA.SetDataEffectiveDateFrom(request.DataEffectiveDateFrom);
            doctorFeesUHIA.SetDataEffectiveDateTo(request.DataEffectiveDateTo);


            return (await doctorFeesUHIA.Update(_doctorFeesUHIARepository, _validationEngine,_identityProvider.GetUserName()));
        }
    }
}
