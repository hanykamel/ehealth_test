using EHealth.ManageItemLists.Domain.ItemTypes;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.ItemListTypes
{
    public class ItemListTypeValidator : AbstractValidator<ItemListType>
    {
        public ItemListTypeValidator()
        {
            RuleFor(x => x.NameAr).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.NameEN).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);

        }


    }
}
