using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackageComponent.Commnads.Handlers
{
    using InvestmentCostPackageComponent = Domain.Packages.InvestmentCostPackage.InvestmentCostPackageComponents.InvestmentCostPackageComponent;

    internal class DeleteInvestmentCostFacilityHandler : IRequestHandler<DeleteInvestmentCostFacilityCommand, bool>
    {
        private readonly IInvestmentCostPackageComponentRepository _investmentCostPackageComponentRepository;
        private readonly IValidationEngine _validationEngine;

        public DeleteInvestmentCostFacilityHandler(IInvestmentCostPackageComponentRepository investmentCostPackageComponentRepository,
            IValidationEngine validationEngine)
        {
            _validationEngine = validationEngine;
            _investmentCostPackageComponentRepository = investmentCostPackageComponentRepository;
        }
        public async Task<bool> Handle(DeleteInvestmentCostFacilityCommand request, CancellationToken cancellationToken)
        {
            _validationEngine.Validate(request);
            var investmentCosts = await InvestmentCostPackageComponent.Search(_investmentCostPackageComponentRepository, x => x.PackageHeaderId == request.PackageHeaderId, 1, 1, false, null, null);
            var investmentCost = investmentCosts.Data.FirstOrDefault();
            investmentCost?.SetFacilityUHIANull();
            return await investmentCost?.Update(_investmentCostPackageComponentRepository,_validationEngine);
        }
    }
}
