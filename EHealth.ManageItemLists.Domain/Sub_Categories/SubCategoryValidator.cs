using EHealth.ManageItemLists.Domain.Categories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Sub_Categories
{
    public class SubCategoryValidator : AbstractValidator<SubCategory>
    {
        public SubCategoryValidator()
        {

            RuleFor(x => x.SubCategoryAr).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.SubCategoryEn).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.DefinitionAr).MinimumLength(1).MaximumLength(1500);
            RuleFor(x => x.DefinitionEn).MinimumLength(1).MaximumLength(1500);

        }
    }
}
