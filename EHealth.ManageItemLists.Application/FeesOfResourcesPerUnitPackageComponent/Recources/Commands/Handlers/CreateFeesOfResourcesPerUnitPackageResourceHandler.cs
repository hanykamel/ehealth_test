using DocumentFormat.OpenXml.Bibliography;
using EHealth.ManageItemLists.Domain.Resource.UHIA;
using EHealth.ManageItemLists.Domain.Shared.Identity;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using MediatR;

namespace EHealth.ManageItemLists.Application.FeesOfResourcesPerUnitPackageComponent.Recources.Commands.Handlers
{
    public class CreateFeesOfResourcesPerUnitPackageResourceHandler : IRequestHandler<CreateFeesOfResourcesPerUnitPackageResourceCommand, Guid>
    {
        private readonly IFeesOfResourcesPerUnitPackageResourceRepository _feesOfResourcesPerUnitPackageResourceRepository;
        private readonly IResourceUHIARepository _resourceUHIARepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;
        public CreateFeesOfResourcesPerUnitPackageResourceHandler(IFeesOfResourcesPerUnitPackageResourceRepository feesOfResourcesPerUnitPackageResourceRepository,
            IValidationEngine validationEngine,
            IIdentityProvider identityProvider,
            IResourceUHIARepository resourceUHIARepository)
        {
            _feesOfResourcesPerUnitPackageResourceRepository = feesOfResourcesPerUnitPackageResourceRepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
            _resourceUHIARepository = resourceUHIARepository;
        }
        public async Task<Guid> Handle(CreateFeesOfResourcesPerUnitPackageResourceCommand request, CancellationToken cancellationToken)
        {
            var resourceUhia = await ResourceUHIA.Get(request.ResourceUHIAId, _resourceUHIARepository);
           
            var DailyCostOfTheResource = resourceUhia.ItemListPrices.OrderByDescending(x => x.EffectiveDateFrom).FirstOrDefault()?.Price
                 / resourceUhia.ItemListPrices.OrderByDescending(x => x.EffectiveDateFrom).FirstOrDefault()?.PriceUnit.ResourceUnitOfCostValue;
            var TotalDailyCostOfResourcePerFacility = DailyCostOfTheResource * request.Quantity;

            var feesOfResourcesPerUnitPackageResource = request.ToFeesOfResourcesPerUnitPackageResource(_identityProvider.GetUserName(), _identityProvider.GetTenantId(), DailyCostOfTheResource,
               TotalDailyCostOfResourcePerFacility);
            return (await feesOfResourcesPerUnitPackageResource.Create(_feesOfResourcesPerUnitPackageResourceRepository, _validationEngine));
        }
    }

}
