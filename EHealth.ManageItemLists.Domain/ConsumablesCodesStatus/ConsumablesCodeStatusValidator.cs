using FluentValidation;


namespace EHealth.ManageItemLists.Domain.ConsumablesCodesStatus
{
    public class ConsumablesCodeStatusValidator : AbstractValidator<ConsumablesCodeStatus>
    {
        public ConsumablesCodeStatusValidator()
        {
            RuleFor(x => x.Code).NotEmpty().NotNull();
            RuleFor(x => x.CodeStatusDescAr).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.CodeStatusDescEng).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
        }
    }
}
