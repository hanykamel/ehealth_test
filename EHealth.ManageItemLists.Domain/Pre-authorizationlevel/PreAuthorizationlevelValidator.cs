using FluentValidation;


namespace EHealth.ManageItemLists.Domain.Pre_authorizationlevel
{
    public class PreAuthorizationlevelValidator : AbstractValidator<PreAuthorizationlevel>
    {
        public PreAuthorizationlevelValidator()
        {
            RuleFor(x => x.LevelAr).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.LevelENG).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.DefinitionAr).MinimumLength(1).MaximumLength(1500);
            RuleFor(x => x.DefinitionENG).MinimumLength(1).MaximumLength(1500);
        }
    }
}
