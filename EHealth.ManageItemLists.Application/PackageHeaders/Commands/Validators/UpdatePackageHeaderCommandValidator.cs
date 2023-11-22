using EHealth.ManageItemLists.Domain.Packages.PackageHeaders;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.PackageHeaders.Commands.Validators
{
    public class UpdatePackageHeaderCommandValidator : AbstractValidator<UpdatePackageHeaderCommand>
    {
        private readonly IPackageHeaderRepository _packageHeaderRepository;
        private bool _validPackageHeader = false;
        public UpdatePackageHeaderCommandValidator(IPackageHeaderRepository packageHeaderRepository)
        {
            _packageHeaderRepository = packageHeaderRepository;

            RuleFor(x => x.Id).MustAsync(async (PackageHeaderId, CancellationToken) =>
            {
                try
                {
                    var packageHeader = await PackageHeader.Get(PackageHeaderId, _packageHeaderRepository);
                    if (packageHeader is not null)
                    {
                        _validPackageHeader = true;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }).WithErrorCode("PackageHeaderNotExist").WithMessage("PackageHeader with PackageHeaderId not exist.")
                .When(x => !string.IsNullOrEmpty(x.Id.ToString()));
        }
    }
}
