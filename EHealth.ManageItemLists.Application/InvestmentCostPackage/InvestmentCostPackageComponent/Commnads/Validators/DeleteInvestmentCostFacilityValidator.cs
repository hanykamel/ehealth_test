﻿using EHealth.ManageItemLists.Domain.Packages.PackageHeaders;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using FluentValidation;

namespace EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackageComponent.Commnads.Validators
{
    public class DeleteInvestmentCostFacilityValidator : AbstractValidator<DeleteInvestmentCostFacilityCommand>
    {

    private readonly IPackageHeaderRepository _packageHeaderRepository;

    public DeleteInvestmentCostFacilityValidator(IPackageHeaderRepository packageHeaderRepository)
    {
        _packageHeaderRepository = packageHeaderRepository;

        RuleFor(x => x.PackageHeaderId).MustAsync(async (PackageHeaderId, CancellationToken) =>
        {

            try
            {
                var packageHeader = await PackageHeader.Get(PackageHeaderId, _packageHeaderRepository);
                if (packageHeader is not null)
                {
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
            .When(x => !string.IsNullOrEmpty(x.PackageHeaderId.ToString()));
        
    }
}
}
