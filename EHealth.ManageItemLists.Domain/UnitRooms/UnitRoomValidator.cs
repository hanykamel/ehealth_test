using FluentValidation;

namespace EHealth.ManageItemLists.Domain.UnitRooms
{
    public class UnitRoomValidator : AbstractValidator<UnitRoom>
    {
        public UnitRoomValidator()
        {
            RuleFor(x => x.NameAr).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.NameEN).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.DefinitionAr).MinimumLength(1).MaximumLength(1500);
            RuleFor(x => x.DefinitionEN).MinimumLength(1).MaximumLength(1500);
        }
    }
}
