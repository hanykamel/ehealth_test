using EHealth.ManageItemLists.Domain.ItemListPricing;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Resource.ItemPrice
{
    public class ResourceItemPriceValidator : AbstractValidator<ResourceItemPrice>
    {
        public ResourceItemPriceValidator()
        {
            RuleFor(x => x.Price).NotEmpty().NotNull().GreaterThan(0);
            RuleFor(x => x.PriceUnitId).NotEmpty().NotNull();
            RuleFor(x => x.EffectiveDateFrom).NotEmpty().NotNull();
            RuleFor(x => x.EffectiveDateTo).Must((model, EffectiveDateTo) =>
            {
                if (model.EffectiveDateFrom < EffectiveDateTo.Value) { return true; }
                else return false;
            }).When(x => x.EffectiveDateTo.HasValue);
        }
    }
}
