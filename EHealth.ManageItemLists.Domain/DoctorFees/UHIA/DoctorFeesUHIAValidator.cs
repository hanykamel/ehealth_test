using EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.DoctorFees.UHIA
{
    public class DoctorFeesUHIAValidator : AbstractValidator<DoctorFeesUHIA>
    {
        public DoctorFeesUHIAValidator()
        {
            RuleFor(x => x.Code).NotNull().NotEmpty();
            RuleFor(x => x.DescriptorAr).MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.DescriptorEn).NotNull().NotEmpty().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.PackageComplexityClassificationId).NotNull().NotEmpty();
            RuleFor(x => x.DataEffectiveDateFrom).NotNull().NotEmpty();
            RuleFor(x => x.DataEffectiveDateTo).Must((model, EffectiveDateTo) =>
            {
                if (model.DataEffectiveDateFrom < EffectiveDateTo.Value) { return true; }
                else return false;
            }).When(x => x.DataEffectiveDateTo.HasValue);
        }
    }
}
