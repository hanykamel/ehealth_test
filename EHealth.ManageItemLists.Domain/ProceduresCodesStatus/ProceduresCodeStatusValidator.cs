using FluentValidation;


namespace EHealth.ManageItemLists.Domain.ProceduresCodesStatus
{
    public class ProceduresCodeStatusValidator : AbstractValidator<ProceduresCodeStatus>
    {
        public ProceduresCodeStatusValidator()
        {
            RuleFor(x => x.Code).NotEmpty().NotNull();
            RuleFor(x => x.CodeStatusDescAr).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.CodeStatusDescEng).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
        }
    }
}
