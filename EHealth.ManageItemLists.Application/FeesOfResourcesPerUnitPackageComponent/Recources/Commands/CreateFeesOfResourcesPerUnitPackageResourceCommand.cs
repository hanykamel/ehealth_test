using EHealth.ManageItemLists.Application.FeesOfResourcesPerUnitPackageComponent.Recources.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;

namespace EHealth.ManageItemLists.Application.FeesOfResourcesPerUnitPackageComponent.Recources.Commands
{
    public class CreateFeesOfResourcesPerUnitPackageResourceCommand : CreateFeesOfResourcesPerUnitPackageResourceDto, IRequest<Guid>
    {
        private readonly IFeesOfResourcesPerUnitPackageResourceRepository _feesOfResourcesPerUnitPackageResourceRepository;
        public CreateFeesOfResourcesPerUnitPackageResourceCommand(CreateFeesOfResourcesPerUnitPackageResourceDto request, IFeesOfResourcesPerUnitPackageResourceRepository feesOfResourcesPerUnitPackageResourceRepository)
        {
            FeesOfResourcesPerUnitPackageComponentId = request.FeesOfResourcesPerUnitPackageComponentId;
            ResourceUHIAId = request.ResourceUHIAId;
            Quantity = request.Quantity;
            _feesOfResourcesPerUnitPackageResourceRepository= feesOfResourcesPerUnitPackageResourceRepository;
        }

    }
}
