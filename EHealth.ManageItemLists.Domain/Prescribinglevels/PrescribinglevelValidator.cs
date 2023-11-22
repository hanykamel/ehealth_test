
using FluentValidation;


namespace EHealth.ManageItemLists.Domain.Prescribinglevels
{
    public class PrescribinglevelValidator : AbstractValidator<Prescribinglevel>
    {
        public PrescribinglevelValidator()
        {
            
            RuleFor(x => x.LevelAr).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.LevelENG).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.DefinitionAr).MinimumLength(1).MaximumLength(1500);
            RuleFor(x => x.DefinitionENG).MinimumLength(1).MaximumLength(1500);
        }
    }
}
