using EHealth.ManageItemLists.Domain.Resource.ItemPrice;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.DoctorFees.ItemPrice
{
    public class DoctorFeesItemPriceValidator : AbstractValidator<DoctorFeesItemPrice>
    {
        public DoctorFeesItemPriceValidator()
        {
            RuleFor(x => x.DoctorFees).NotEmpty().NotNull().GreaterThan(0);
            RuleFor(x => x.UnitOfDoctorFeesId).NotEmpty().NotNull();
            RuleFor(x => x.EffectiveDateFrom).NotEmpty().NotNull();
            RuleFor(x => x.EffectiveDateTo).Must((model, EffectiveDateTo) =>
            {
                if (model.EffectiveDateFrom < EffectiveDateTo.Value) { return true; }
                else return false;
            }).When(x => x.EffectiveDateTo.HasValue);
        }
    }
}
