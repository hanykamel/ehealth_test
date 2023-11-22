using FluentValidation;

namespace EHealth.ManageItemLists.Domain.MappingTypes
{
    public class MappingTypeValidator : AbstractValidator<MappingType>
    {
        public MappingTypeValidator()
        {
            RuleFor(x => x.MappingTypeAr).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.MappingTypeENG).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.DefinitionAr).MinimumLength(1).MaximumLength(1500);
            RuleFor(x => x.DefinitionENG).MinimumLength(1).MaximumLength(1500);
        }
    }
}
