using FluentValidation;


namespace EHealth.ManageItemLists.Domain.RegistrationTypes
{
    public class RegistrationTypeValidator:AbstractValidator<RegistrationType>
    {
        public RegistrationTypeValidator()
        {
            RuleFor(x => x.RegistrationTypeAr).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.RegistrationTypeENG).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.DefinitionAr).MinimumLength(1).MaximumLength(1500);
            RuleFor(x => x.DefinitionENG).MinimumLength(1).MaximumLength(1500);
        }
    }
}
