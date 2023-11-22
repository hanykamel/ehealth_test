using EHealth.ManageItemLists.Domain.PackageComplexityClassifications;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Locations
{
    internal class LocationValidator : AbstractValidator<Location>
    {
        public LocationValidator()
        {

            RuleFor(x => x.LocationAr).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.LocationEn).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.DefinitionAr).MinimumLength(1).MaximumLength(1500);
            RuleFor(x => x.DefinitionEn).MinimumLength(1).MaximumLength(1500);
        }
    }
}
