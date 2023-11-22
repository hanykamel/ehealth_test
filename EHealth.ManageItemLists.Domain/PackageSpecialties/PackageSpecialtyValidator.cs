﻿using EHealth.ManageItemLists.Domain.GlobelPackageTypes;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.PackageSpecialties
{
    public class PackageSpecialtyValidator : AbstractValidator<PackageSpecialty>
    {
        public PackageSpecialtyValidator()
        {
            RuleFor(x => x.SpecialtyAr).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.SpecialtyEn).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.DefinitionAr).Length(1, 1500).When(x => !string.IsNullOrEmpty(x.DefinitionAr));
            RuleFor(x => x.DefinitionEn).Length(1, 1500).When(x => !string.IsNullOrEmpty(x.DefinitionEn));
        }
    }
}
