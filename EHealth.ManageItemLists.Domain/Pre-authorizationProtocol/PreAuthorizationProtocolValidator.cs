using FluentValidation;


namespace EHealth.ManageItemLists.Domain.Pre_authorizationProtocol
{
    public class PreAuthorizationProtocolValidator : AbstractValidator<PreAuthorizationProtocol>
    {
        public PreAuthorizationProtocolValidator()
        {
            RuleFor(x => x.NameAr).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.NameENG).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.DefinitionAr).MinimumLength(1).MaximumLength(1500);
            RuleFor(x => x.DefinitionENG).MinimumLength(1).MaximumLength(1500);
        }
    }
}
