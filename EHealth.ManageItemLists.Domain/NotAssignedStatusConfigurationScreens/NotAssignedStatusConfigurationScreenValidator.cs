using EHealth.ManageItemLists.Domain.Rejectreasons;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.NotAssignedStatusConfigurationScreens
{
    public class NotAssignedStatusConfigurationScreenValidator : AbstractValidator<NotAssignedStatusConfigurationScreen>
    {
        public NotAssignedStatusConfigurationScreenValidator()
        {

            RuleFor(x => x.TotalNumberOfDays).LessThanOrEqualTo(99);
            RuleFor(x => x.SendNotificationEvery).LessThanOrEqualTo(99).LessThan(x => x.TotalNumberOfDays);
        }
    }
}
