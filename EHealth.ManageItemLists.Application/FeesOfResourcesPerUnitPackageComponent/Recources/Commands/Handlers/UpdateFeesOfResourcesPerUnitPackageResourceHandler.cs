using EHealth.ManageItemLists.Domain.Facility.UHIA;
using EHealth.ManageItemLists.Domain.Packages.FeesOfResourcesPerUnitPackage.FeesOfResourcesPerUnitPackageResources;
using EHealth.ManageItemLists.Domain.Resource.UHIA;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Identity;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using MediatR;

namespace EHealth.ManageItemLists.Application.FeesOfResourcesPerUnitPackageComponent.Recources.Commands.Handlers
{
    public class UpdateFeesOfResourcesPerUnitPackageResourceHandler : IRequestHandler<UpdateFeesOfResourcesPerUnitPackageResourceCommand, bool>
    {
        private readonly IFeesOfResourcesPerUnitPackageResourceRepository _feesOfResourcesPerUnitPackageResourceRepository;
        private readonly IResourceUHIARepository _resourceUHIARepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;
        public UpdateFeesOfResourcesPerUnitPackageResourceHandler(IFeesOfResourcesPerUnitPackageResourceRepository feesOfResourcesPerUnitPackageResourceRepository,
            IValidationEngine validationEngine,
            IIdentityProvider identityProvider,
            IResourceUHIARepository resourceUHIARepository)
        {
            _feesOfResourcesPerUnitPackageResourceRepository = feesOfResourcesPerUnitPackageResourceRepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
            _resourceUHIARepository = resourceUHIARepository;
        }
        public async Task<bool> Handle(UpdateFeesOfResourcesPerUnitPackageResourceCommand request, CancellationToken cancellationToken)
        {
            _validationEngine.Validate(request);
            var resourceUhia = await ResourceUHIA.Get(request.ResourceUHIAId, _resourceUHIARepository);

            var DailyCostOfTheResource = resourceUhia.ItemListPrices.OrderByDescending(x => x.EffectiveDateFrom).FirstOrDefault()?.Price
                 / resourceUhia.ItemListPrices.OrderByDescending(x => x.EffectiveDateFrom).FirstOrDefault()?.PriceUnit.ResourceUnitOfCostValue;
            var TotalDailyCostOfResourcePerFacility = DailyCostOfTheResource * request.Quantity;
            var feesOfResourcesPerUnitPackageResource =await FeesOfResourcesPerUnitPackageResource.Get(request.Id, _feesOfResourcesPerUnitPackageResourceRepository);
            if (feesOfResourcesPerUnitPackageResource == null)
                throw new DataNotFoundException();

            feesOfResourcesPerUnitPackageResource.SetResourceUHIAId(request.ResourceUHIAId);
            feesOfResourcesPerUnitPackageResource.SetQuantity(request.Quantity);
            feesOfResourcesPerUnitPackageResource.SetDailyCostOfTheResource(DailyCostOfTheResource);
            feesOfResourcesPerUnitPackageResource.SetTotalDailyCostOfResourcePerFacility(TotalDailyCostOfResourcePerFacility);
            return (await feesOfResourcesPerUnitPackageResource.Update(_feesOfResourcesPerUnitPackageResourceRepository, _validationEngine));
        }
    }
}
