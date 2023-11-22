using FluentValidation;


namespace EHealth.ManageItemLists.Domain.PriceUnits
{
    public class PriceUnitValidator : AbstractValidator<PriceUnit>
    {
        public PriceUnitValidator()
        {
            RuleFor(x => x.NameAr).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.NameEN).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.DefinitionAr).MinimumLength(1).MaximumLength(1500);
            RuleFor(x => x.DefinitionEN).MinimumLength(1).MaximumLength(1500);
            RuleFor(x => x.ResourceUnitOfCostValue).NotEmpty().NotNull().GreaterThan(0).LessThanOrEqualTo(100);
        }
    }
}
