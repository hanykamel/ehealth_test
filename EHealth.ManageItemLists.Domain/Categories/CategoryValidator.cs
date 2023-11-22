using FluentValidation;

namespace EHealth.ManageItemLists.Domain.Categories
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            
            RuleFor(x => x.CategoryAr).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.CategoryEn).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.DefinitionAr).MinimumLength(1).MaximumLength(1500);
            RuleFor(x => x.DefinitionEn).MinimumLength(1).MaximumLength(1500);

        }
    }
}
