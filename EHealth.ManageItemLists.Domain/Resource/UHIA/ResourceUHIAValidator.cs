using EHealth.ManageItemLists.Domain.Facility.UHIA;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Resource.UHIA
{
    public class ResourceUHIAValidator : AbstractValidator<ResourceUHIA>
    {
        public ResourceUHIAValidator()
        {
            RuleFor(x => x.Code).NotNull().NotEmpty();
            RuleFor(x => x.DescriptorAr).Length(1, 100).When(x => !string.IsNullOrEmpty(x.DescriptorAr));
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
