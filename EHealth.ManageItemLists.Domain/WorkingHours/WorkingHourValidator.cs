using EHealth.ManageItemLists.Domain.PublicVacations;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.WorkingHours
{
    public class WorkingHourValidator : AbstractValidator<WorkingHour>
    {
        public WorkingHourValidator()
        {
            RuleFor(x => x.FromTime).NotEmpty().NotNull();
            RuleFor(x => x.ToTime).NotEmpty().NotNull();
            RuleFor(x => x.NonWorkingDays).NotEmpty().NotNull();
        }
    }
}
