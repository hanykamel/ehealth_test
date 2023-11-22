using EHealth.ManageItemLists.Domain.Prescribinglevels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.PackageComplexityClassifications
{
    public class PackageComplexityClassificationValidator : AbstractValidator<PackageComplexityClassification>
    {
        public PackageComplexityClassificationValidator()
        {

            RuleFor(x => x.ComplexityAr).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.ComplexityEn).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.DefinitionAr).MinimumLength(1).MaximumLength(1500);
            RuleFor(x => x.DefinitionEn).MinimumLength(1).MaximumLength(1500);
        }
    }
}
