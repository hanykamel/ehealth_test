using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.DevicesAndAssets.UHIA
{
    public class DevicesAndAssetsUHIAValidator : AbstractValidator<DevicesAndAssetsUHIA>
    {
        public DevicesAndAssetsUHIAValidator()
        {
            RuleFor(x => x.Code).NotNull().NotEmpty();
            RuleFor(x => x.DescriptorAr).NotNull().NotEmpty().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.DescriptorEn).NotNull().NotEmpty().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.CategoryId).NotNull().NotEmpty();
            RuleFor(x => x.SubCategoryId).NotNull().NotEmpty();
            RuleFor(x => x.DataEffectiveDateFrom).NotNull().NotEmpty();
            RuleFor(x => x.DataEffectiveDateTo).Must((model, EffectiveDateTo) =>
            {
                if (model.DataEffectiveDateFrom < EffectiveDateTo.Value) { return true; }
                else return false;
            }).When(x => x.DataEffectiveDateTo.HasValue);
        }
    }
}
