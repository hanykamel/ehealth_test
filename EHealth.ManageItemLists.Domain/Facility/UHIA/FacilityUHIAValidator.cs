using EHealth.ManageItemLists.Domain.Services.ServicesUHIA;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Facility.UHIA
{
    public class FacilityUHIAValidator : AbstractValidator<FacilityUHIA>
    {
        public FacilityUHIAValidator()
        {
            RuleFor(x => x.Code).NotNull().NotEmpty().MinimumLength(1).MaximumLength(500);
            //RuleFor(x => x.DescriptorAr).MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.DescriptorEn).NotNull().NotEmpty().MinimumLength(1).MaximumLength(100);

            RuleFor(x => x.OccupancyRate).GreaterThan(0).Must(x => x?.ToString().Length >= 2 && x?.ToString().Length <= 4).When(x => x.OccupancyRate.HasValue);
            RuleFor(x => x.OperatingRateInHoursPerDay).GreaterThan(0).Must(x => x.ToString().Length >= 2 && x.ToString().Length <= 4);
            RuleFor(x => x.OperatingDaysPerMonth).GreaterThan(0).Must(x => x.ToString().Length >= 2 && x.ToString().Length <= 4);

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
