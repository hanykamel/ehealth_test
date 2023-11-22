using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.DrugsPricing
{
    public class DrugPriceValidator:AbstractValidator<DrugPrice>
    {
        public DrugPriceValidator()
        {
            RuleFor(x=>x.MainUnitPrice).NotNull().NotEmpty();   
            RuleFor(x=>x.FullPackPrice).NotNull().NotEmpty();   
            RuleFor(x=>x.SubUnitPrice).NotNull().NotEmpty();
            RuleFor(x=>x.EffectiveDateFrom).NotNull().NotEmpty();
            RuleFor(x => x.EffectiveDateTo).Must((model, EffectiveDateTo) =>
            {
                if (model.EffectiveDateFrom < EffectiveDateTo.Value) { return true; }
                else return false;
            }).When(x => x.EffectiveDateTo.HasValue);
        }
    }
}
