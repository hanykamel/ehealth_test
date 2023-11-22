using EHealth.ManageItemLists.Domain.LocalTypeOfMeasure;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.PublicVacations
{
    public class PublicVacationValidator : AbstractValidator<PublicVacation>
    {
        public PublicVacationValidator()
        {
            RuleFor(x => x.NameAr).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.NameEn).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.FromDate).GreaterThan(DateTime.Now);
            RuleFor(x => x.ToDate).GreaterThan(DateTime.Now);
            RuleFor(x => x.ToDate).GreaterThan(x => x.FromDate);
        }
    }
}
