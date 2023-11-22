using FluentValidation;

namespace EHealth.ManageItemLists.Domain.ReimbursementCategories
{
    public class ReimbursementCategoryValidator : AbstractValidator<ReimbursementCategory>
    {
        public ReimbursementCategoryValidator()
        {
            RuleFor(x => x.NameAr).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.NameENG).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.DefinitionAr).MinimumLength(1).MaximumLength(1500);
            RuleFor(x => x.DefinitionENG).MinimumLength(1).MaximumLength(1500);
        }
    }
}
