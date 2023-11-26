using EHealth.ManageItemLists.Domain.Packages.PackageHeaders;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using FluentValidation;

namespace EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackageComponent.Commnads.Validators
{
    using FacilityUHIA = Domain.Facility.UHIA.FacilityUHIA;
    public class SetInvestmentCostComponentValidator : AbstractValidator<SetInvestmentCostComponentCommand>
    {
        private readonly IFacilityUHIARepository _facilityUHIARepository;
        private readonly IPackageHeaderRepository _packageHeaderRepository;

        public SetInvestmentCostComponentValidator(IPackageHeaderRepository packageHeaderRepository,
            IFacilityUHIARepository facilityUHIARepository)
        {
            _packageHeaderRepository = packageHeaderRepository;
            _facilityUHIARepository = facilityUHIARepository;

            RuleFor(x => x.PackageHeaderId).MustAsync(async (PackageHeaderId, CancellationToken) =>
            {
                
                try
                {
                    var packageHeader = await PackageHeader.Get(PackageHeaderId, _packageHeaderRepository);
                    if (packageHeader is not null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }).WithErrorCode("PackageHeaderNotExist").WithMessage("PackageHeader with PackageHeaderId not exist.")
                .When(x => !string.IsNullOrEmpty(x.PackageHeaderId.ToString()));

            RuleFor(x => x.FacilityUHIAId).MustAsync(async (FacilityUHIAId, CancellationToken) =>
            {
                try
                {
                    var facilityUHIA = await FacilityUHIA.Get(FacilityUHIAId, _facilityUHIARepository);
                    if (facilityUHIA is not null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }).WithErrorCode("FacilityUHIANotExist").WithMessage("FacilityUHIA with FacilityUHIAId not exist.")
                .When(x => !string.IsNullOrEmpty(x.FacilityUHIAId.ToString()));
        }
    }
}
