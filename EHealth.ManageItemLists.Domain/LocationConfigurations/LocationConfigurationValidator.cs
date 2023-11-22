using EHealth.ManageItemLists.Domain.PackageSubTypes;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.LocationConfigurations
{
    public class LocationConfigurationValidator : AbstractValidator<LocationConfiguration>
    {
        public LocationConfigurationValidator()
        {
            RuleFor(x => x.NeededLocation).NotEmpty().NotNull();
            RuleFor(x => x.PackageSubTypeId).NotEmpty().NotNull().GreaterThan(0);

        }

    }
}
