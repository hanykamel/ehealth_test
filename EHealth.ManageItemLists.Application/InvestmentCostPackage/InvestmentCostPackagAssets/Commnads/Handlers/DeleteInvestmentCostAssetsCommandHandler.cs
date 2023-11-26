using EHealth.ManageItemLists.Application.Resource.UHIA.Commands;
using EHealth.ManageItemLists.Domain.Packages.InvestmentCostPackage.InvestmentCostPackagAssets;
using EHealth.ManageItemLists.Domain.Resource.UHIA;
using EHealth.ManageItemLists.Domain.Shared.Exceptions;
using EHealth.ManageItemLists.Domain.Shared.Identity;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.InvestmentCostPackage.InvestmentCostPackagAssets.Commnads.Handlers
{
    public class DeleteInvestmentCostAssetsCommandHandler : IRequestHandler<DeleteInvestmentCostAssetsCommand, bool>
    {
        private readonly IInvestmentCostPackageAssetRepository _investmentCostPackageAssetRepository;
        private readonly IValidationEngine _validationEngine;
        private readonly IIdentityProvider _identityProvider;

        public DeleteInvestmentCostAssetsCommandHandler(IInvestmentCostPackageAssetRepository investmentCostPackageAssetRepository, IValidationEngine validationEngine, IIdentityProvider identityProvider)
        {
            _investmentCostPackageAssetRepository = investmentCostPackageAssetRepository;
            _validationEngine = validationEngine;
            _identityProvider = identityProvider;
        }
        public async Task<bool> Handle(DeleteInvestmentCostAssetsCommand request, CancellationToken cancellationToken)
        {
            var investmentCostPackageAsset = await InvestmentCostPackageAsset.Get(request.Id, _investmentCostPackageAssetRepository);
            if (investmentCostPackageAsset is not null)
            {
                investmentCostPackageAsset.SoftDelete(_identityProvider.GetUserName());

                return (await investmentCostPackageAsset.Delete(_investmentCostPackageAssetRepository, _validationEngine));
            }
            else { throw new DataNotFoundException(); }

        }
    }
}
