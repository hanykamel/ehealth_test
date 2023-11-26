
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackageComponents;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.PackageHeaders.Commands.Validators
{
    public class DeletePackageHeaderCommandValidator: AbstractValidator<DeletePackageHeaderCommand>
    {
        private bool _validPackageHeader = false;
        private readonly IPackageHeaderRepository _packageHeaderRepository;
        private readonly IInvestmentCostPackageComponentRepository _investmentCostPackageComponentRepository;

        public DeletePackageHeaderCommandValidator(IPackageHeaderRepository packageHeaderRepository,IInvestmentCostPackageComponentRepository investmentCostPackageComponentRepository )
        {
            _packageHeaderRepository = packageHeaderRepository;
            _investmentCostPackageComponentRepository = investmentCostPackageComponentRepository;
            RuleFor(x => x.Id).MustAsync(async (Id, CancellationToken) =>
            {
                try
                {
                    var investmentCost = await InvestmentCostPackageComponent.Search(_investmentCostPackageComponentRepository, x => x.IsDeleted == false && x.PackageHeaderId == Id, 0, 0, false, null, null);
                    if (investmentCost.Data.Count > 0)
                    {
                        _validPackageHeader = false;
                        return false;
                    }
                    return true;
                }
                catch (Exception)
                {

                    return false;
                }
            }).WithErrorCode("Packages_MSG_03");
                
                
        }
    }
}
