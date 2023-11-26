using EHealth.ManageItemLists.Domain.Packages.FeesOfResourcesPerUnitPackage.FeesOfResourcesPerUnitPackageResources;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using FluentValidation;

namespace EHealth.ManageItemLists.Application.FeesOfResourcesPerUnitPackageComponent.Recources.Commands.Handlers.Validators
{
    public class UpdateFeesOfResourcesPerUnitPackageResourceValidator : AbstractValidator<UpdateFeesOfResourcesPerUnitPackageResourceCommand>
    {
        private readonly IFeesOfResourcesPerUnitPackageResourceRepository _feesOfResourcesPerUnitPackageResourceRepository;
        private bool _valid = false;
        public UpdateFeesOfResourcesPerUnitPackageResourceValidator(IFeesOfResourcesPerUnitPackageResourceRepository feesOfResourcesPerUnitPackageResourceRepository)
        {
            _feesOfResourcesPerUnitPackageResourceRepository= feesOfResourcesPerUnitPackageResourceRepository;
            RuleFor(x => x.Id).MustAsync(async (Id, CancellationToken) =>
            {
                try
                {
                    var feesOfResourcesPerUnitPackageResource = await FeesOfResourcesPerUnitPackageResource.Get(Id, _feesOfResourcesPerUnitPackageResourceRepository);
                    if (feesOfResourcesPerUnitPackageResource is not null)
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
            }).WithErrorCode("feesOfResourcesPerUnitPackageResourceNotExist").WithMessage("feesOfResourcesPerUnitPackageResource with this Id not exist.")
              .When(x => !string.IsNullOrEmpty(x.Id.ToString()));
        }
    }
}
