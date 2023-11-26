using EHealth.ManageItemLists.Domain.Shared.Identity;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using ICSharpCode.SharpZipLib.Zip;
using MediatR;

namespace EHealth.ManageItemLists.Application.FeesOfResourcesPerUnitPackageComponent.Facility.Commands.Handlers
{
    public class CreateFeesOfResourcesPerUnitPackageHandler : IRequestHandler<CreateFeesOfResourcesPerUnitPackageCommand, Guid>
    {
        private readonly IFeesOfResourcesPerUnitPackageComponentRepository _feesOfResourcesPerUnitPackageComponentRepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;
        public CreateFeesOfResourcesPerUnitPackageHandler(IFeesOfResourcesPerUnitPackageComponentRepository feesOfResourcesPerUnitPackageComponentRepository,
        IValidationEngine validationEngine,
        IIdentityProvider identityProvider)
        {
            _feesOfResourcesPerUnitPackageComponentRepository = feesOfResourcesPerUnitPackageComponentRepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
        }
        public async Task<Guid> Handle(CreateFeesOfResourcesPerUnitPackageCommand request, CancellationToken cancellationToken)
        {
            //_validationEngine.Validate(request);


            var FeesOfResourcesPerUnitPackage = await Domain.Packages.FeesOfResourcesPerUnitPackage
                .FeesOfResourcesPerUnitPackageComponents.FeesOfResourcesPerUnitPackageComponent
                .Search(_feesOfResourcesPerUnitPackageComponentRepository, x => x.PackageHeaderId == request.PackageHeaderId &&
                x.IsDeleted != true, 1, 1, false, null, null);
            if (FeesOfResourcesPerUnitPackage.TotalCount > 0)
            {
                FeesOfResourcesPerUnitPackage.Data.LastOrDefault().SetFacilityUHIAId(request.FacilityUHIAId);
                FeesOfResourcesPerUnitPackage.Data.LastOrDefault().SetNumberOfSessionsPerUnitPerFacility(request.NumberOfSessionsPerUnitPerFacility);
                FeesOfResourcesPerUnitPackage.Data.LastOrDefault().SetQuantityOfUnitsPerTheFacility(request.QuantityOfUnitsPerTheFacility);
                await FeesOfResourcesPerUnitPackage.Data.LastOrDefault().Update(_feesOfResourcesPerUnitPackageComponentRepository, _validationEngine);
                return FeesOfResourcesPerUnitPackage.Data.LastOrDefault().Id;

            }
            else
            {
                var FeesOfResourcesPerUnitPackageNew = request.ToFeesOfResourcesPerUnitPackage(_identityProvider.GetUserName(), _identityProvider.GetTenantId(), null);
                await FeesOfResourcesPerUnitPackageNew.Create(_feesOfResourcesPerUnitPackageComponentRepository, _validationEngine);
                return FeesOfResourcesPerUnitPackageNew.Id;

            }

        }
    }
}
