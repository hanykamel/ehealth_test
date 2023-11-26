using EHealth.ManageItemLists.Application.FeesOfResourcesPerUnitPackageComponent.Facility.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using MediatR;

namespace EHealth.ManageItemLists.Application.FeesOfResourcesPerUnitPackageComponent.Facility.Commands
{

    public class CreateFeesOfResourcesPerUnitPackageCommand : CreateFeesOfResourcesPerUnitPackageDto, IRequest<Guid>/*, IValidationModel<CreateFeesOfResourcesPerUnitPackageCommand>*/
    {
        private readonly IFeesOfResourcesPerUnitPackageComponentRepository _feesOfResourcesPerUnitPackageComponentRepository;
        //private readonly IPackageHeaderRepository _packageHeaderRepository;
        public CreateFeesOfResourcesPerUnitPackageCommand(CreateFeesOfResourcesPerUnitPackageDto request, IFeesOfResourcesPerUnitPackageComponentRepository feesOfResourcesPerUnitPackageComponentRepository)
        {
            //Id = request.Id;
            FacilityUHIAId = request.FacilityUHIAId;
            PackageHeaderId = request.PackageHeaderId;
            QuantityOfUnitsPerTheFacility = request.QuantityOfUnitsPerTheFacility;
            NumberOfSessionsPerUnitPerFacility = request.NumberOfSessionsPerUnitPerFacility;
            _feesOfResourcesPerUnitPackageComponentRepository = feesOfResourcesPerUnitPackageComponentRepository;
            //_packageHeaderRepository = packageHeaderRepository;
        }

        //public AbstractValidator<CreateFeesOfResourcesPerUnitPackageCommand> Validator => new FeesOfResourcesPerUnitPackageValidator(_feesOfResourcesPerUnitPackageComponentRepository,_packageHeaderRepository);
    }
}