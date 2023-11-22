using FluentValidation;


namespace EHealth.ManageItemLists.Domain.DrugsPackageTypes
{
    public class DrugsPackageTypeValidator : AbstractValidator<DrugsPackageType>
    {
        public DrugsPackageTypeValidator()
        {
            RuleFor(x => x.NameAr).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.NameEN).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.DefinitionAr).MinimumLength(1).MaximumLength(1500);
            RuleFor(x => x.DefinitionEN).MinimumLength(1).MaximumLength(1500);
        }
    }
}
