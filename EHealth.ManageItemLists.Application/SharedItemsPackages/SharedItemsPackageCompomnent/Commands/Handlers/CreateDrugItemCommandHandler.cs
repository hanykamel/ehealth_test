using EHealth.ManageItemLists.Domain.Packages.SharedItemsPackages.SharedItemsPackageComponents;
using EHealth.ManageItemLists.Domain.Shared.Identity;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.SharedItemsPackages.SharedItemsPackageCompomnent.Commands.Handlers
{
    public class CreateDrugszItemCommandHandler : IRequestHandler<CreateDrugItemCommand, Guid>
    {
        private readonly IValidationEngine _validationEngine;
        private readonly ISharedItemsPackageComponentRepository _sharedItemsPackageComponentRepository;
        private readonly ISharedItemsPackageDrugRepository _sharedItemsPackageDrugRepository;
        private readonly IIdentityProvider _identityProvider;

        public CreateDrugszItemCommandHandler(IValidationEngine validationEngine,
            ISharedItemsPackageComponentRepository sharedItemsPackageComponentRepository,
            ISharedItemsPackageDrugRepository sharedItemsPackageDrugRepository,
            IIdentityProvider identityProvider)
        {
            _validationEngine = validationEngine;
            _sharedItemsPackageComponentRepository = sharedItemsPackageComponentRepository;
            _sharedItemsPackageDrugRepository = sharedItemsPackageDrugRepository;
            _identityProvider = identityProvider;
        }
        public async Task<Guid> Handle(CreateDrugItemCommand request, CancellationToken cancellationToken)
        {
            _validationEngine.Validate(request);
            var sharedItemsPackageComponents = await SharedItemsPackageComponent.Search(_sharedItemsPackageComponentRepository, x => x.PackageHeaderId == request.PackageHeaderId, 1, 1, false, null, null);
            var sharedItemsPackageComponent = sharedItemsPackageComponents.Data.FirstOrDefault();
            if (sharedItemsPackageComponent == null)
            {
                sharedItemsPackageComponent = SharedItemsPackageComponent.Create(null, request.PackageHeaderId, _identityProvider.GetUserName(), _identityProvider.GetTenantId());
                var sharedItemsPackageComponentId = await sharedItemsPackageComponent.Create(_sharedItemsPackageComponentRepository, _validationEngine);
                await SharedItemsPackageComponent.CreateSharedItemsPackageDrugAsync(_sharedItemsPackageDrugRepository, _validationEngine, sharedItemsPackageComponentId,
                    request.DrugUHIAId, request.Quantity, request.NumberOfCasesInTheUnit, request.LocationId, request.TotalCost,
                    request.DrugPerCase, _identityProvider.GetUserName(), _identityProvider.GetTenantId());
            }
            else
            {
                await SharedItemsPackageComponent.CreateSharedItemsPackageDrugAsync(_sharedItemsPackageDrugRepository, _validationEngine, sharedItemsPackageComponent.Id,
                    request.DrugUHIAId, request.Quantity, request.NumberOfCasesInTheUnit, request.LocationId, request.TotalCost,
                    request.DrugPerCase, _identityProvider.GetUserName(), _identityProvider.GetTenantId());
            }
            return sharedItemsPackageComponent.Id;
        }
    }
}
