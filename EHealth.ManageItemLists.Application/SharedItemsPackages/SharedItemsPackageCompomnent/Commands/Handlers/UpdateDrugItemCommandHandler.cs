using EHealth.ManageItemLists.Domain.Packages.SharedItemsPackages.SharedItemsPackageComponents;
using EHealth.ManageItemLists.Domain.Shared.Repositories;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using MediatR;

namespace EHealth.ManageItemLists.Application.SharedItemsPackages.SharedItemsPackageCompomnent.Commands.Handlers
{
    public class UpdateDrugItemCommandHandler : IRequestHandler<UpdateDrugItemCommand, bool>
    {
        private readonly IValidationEngine _validationEngine;
        private readonly ISharedItemsPackageDrugRepository _sharedItemsPackageDrugRepository;

        public UpdateDrugItemCommandHandler(IValidationEngine validationEngine,
            ISharedItemsPackageDrugRepository sharedItemsPackageDrugRepository
            )
        {
            _validationEngine = validationEngine;
            _sharedItemsPackageDrugRepository = sharedItemsPackageDrugRepository;
        }
        public async Task<bool> Handle(UpdateDrugItemCommand request, CancellationToken cancellationToken)
        {
            _validationEngine.Validate(request);
            return await SharedItemsPackageComponent.UpdateSharedItemsPackageDrugAsync(_sharedItemsPackageDrugRepository, _validationEngine, request.SharedItemsPackageDrugId, request.DrugUHIAId, request.Quantity, request.NumberOfCasesInTheUnit,
                  request.LocationId, request.TotalCost, request.DrugPerCase);
        }
    }
}
