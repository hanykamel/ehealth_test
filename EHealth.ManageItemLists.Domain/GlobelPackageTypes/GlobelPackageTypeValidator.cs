using EHealth.ManageItemLists.Domain.LocalTypeOfMeasure;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.GlobelPackageTypes
{
    public class GlobelPackageTypeValidator : AbstractValidator<GlobelPackageType>
    {
        public GlobelPackageTypeValidator()
        {
            RuleFor(x => x.GlobalTypeAr).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.GlobalTypeEn).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.DefinitionAr).Length(1, 1500).When(x => !string.IsNullOrEmpty(x.DefinitionAr));
            RuleFor(x => x.DefinitionEn).Length(1, 1500).When(x => !string.IsNullOrEmpty(x.DefinitionEn));
        }

    }
}
