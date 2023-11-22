using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.UnitsTypes
{
    public class UnitsTypeValidator:AbstractValidator<UnitsType>
    {
        public UnitsTypeValidator()
        {
            RuleFor(x=>x.UnitAr).NotNull().NotEmpty().MinimumLength(1).MaximumLength(500);
            RuleFor(x=>x.UnitEn).NotNull().NotEmpty().MinimumLength(1).MaximumLength(500);
            RuleFor(x=>x.DefinitionAr).MinimumLength(1).MaximumLength(1500);
            RuleFor(x=>x.DefinitionEn).MinimumLength(1).MaximumLength(1500);
        }
    }
}
