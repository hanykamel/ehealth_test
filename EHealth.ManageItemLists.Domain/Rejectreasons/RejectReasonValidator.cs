using EHealth.ManageItemLists.Domain.PackageComplexityClassifications;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Rejectreasons
{
    public class RejectReasonValidator : AbstractValidator<RejectReason>
    {
        public RejectReasonValidator()
        {

            RuleFor(x => x.RejectReasonAr).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.RejectReasonEn).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
        }
    }
}
