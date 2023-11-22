using EHealth.ManageItemLists.Domain.ItemListSubtypes;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.PackageSubTypes
{
    public class PackageSubTypeValidator : AbstractValidator<PackageSubType>
    {
        public PackageSubTypeValidator()
        {
            RuleFor(x => x.NameAr).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.NameEN).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);

        }

    }
}
