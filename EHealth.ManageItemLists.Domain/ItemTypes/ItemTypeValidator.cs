using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.ItemTypes
{
    public class ItemTypeValidator : AbstractValidator<ItemType>
    {
        public ItemTypeValidator()
        {
            RuleFor(x => x.NameAr).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.NameEN).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.DefinitionAr).MinimumLength(1).MaximumLength(1500);
            RuleFor(x => x.DefinitionEN).MinimumLength(1).MaximumLength(1500);
        }
    }
}
