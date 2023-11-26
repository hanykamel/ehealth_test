using EHealth.ManageItemLists.Domain.Packages.PackageHeaders;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using FluentValidation;

namespace EHealth.ManageItemLists.Application.FeesOfResourcesPerUnitPackageComponent.Facility.Commands.Validators
{
    public class FeesOfResourcesPerUnitPackageValidator : AbstractValidator<CreateFeesOfResourcesPerUnitPackageCommand>
    {
        private readonly IFeesOfResourcesPerUnitPackageComponentRepository _feesOfResourcesPerUnitPackageComponentRepository;
        private readonly IPackageHeaderRepository _packageHeaderRepository;

        private bool _valid = false;
        public FeesOfResourcesPerUnitPackageValidator(IFeesOfResourcesPerUnitPackageComponentRepository feesOfResourcesPerUnitPackageComponentRepository, IPackageHeaderRepository packageHeaderRepository)
        {
            _feesOfResourcesPerUnitPackageComponentRepository= feesOfResourcesPerUnitPackageComponentRepository;
            _packageHeaderRepository= packageHeaderRepository;
            RuleFor(x => x.PackageHeaderId).MustAsync(async (PackageHeaderId, CancellationToken) =>
            {
                try
                {
                    var packageHeader =  await PackageHeader.Get(PackageHeaderId,_packageHeaderRepository);
                    if (packageHeader is not  null)
                    {
                        _valid = true;
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
        }
    }

}
