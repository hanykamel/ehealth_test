using EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackageComponent.Commnads.Validators;
using EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackageComponent.DTOs;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using FluentValidation;
using MediatR;

namespace EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackageComponent.Commnads
{
    public class SetInvestmentCostComponentCommand : SetInvestmentCostComponentDto, IRequest<Guid>, IValidationModel<SetInvestmentCostComponentCommand>
    {
        private readonly IFacilityUHIARepository _facilityUHIARepository;
        private readonly IPackageHeaderRepository _packageHeaderRepository;
        public SetInvestmentCostComponentCommand(SetInvestmentCostComponentDto request, IFacilityUHIARepository facilityUHIARepository, IPackageHeaderRepository packageHeaderRepository)
        {
            _packageHeaderRepository = packageHeaderRepository;
            _facilityUHIARepository = facilityUHIARepository;
            PackageHeaderId = request.PackageHeaderId;
            FacilityUHIAId = request.FacilityUHIAId;
            QuantityOfUnitsPerTheFacility = request.QuantityOfUnitsPerTheFacility;
            NumberOfSessionsPerUnitPerFacility = request.NumberOfSessionsPerUnitPerFacility;
        }
        public AbstractValidator<SetInvestmentCostComponentCommand> Validator => new SetInvestmentCostComponentValidator(_packageHeaderRepository, _facilityUHIARepository);
    }
}
