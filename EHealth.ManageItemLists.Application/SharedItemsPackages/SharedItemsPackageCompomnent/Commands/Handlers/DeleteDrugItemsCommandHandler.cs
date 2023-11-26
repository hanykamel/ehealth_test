using EHealth.ManageItemLists.Domain.Packages.SharedItemsPackages.SharedItemsPackageDrugs;
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
    public class DeleteDrugItemsCommandHandler : IRequestHandler<DeleteDrugItemCommand, bool>
    {
        private readonly ISharedItemsPackageDrugRepository _sharedItemsPackageDrugRepository;
        private readonly IValidationEngine _validationEngine;

        public DeleteDrugItemsCommandHandler(ISharedItemsPackageDrugRepository sharedItemsPackageDrugRepository,
            IValidationEngine validationEngine)
        {
            _sharedItemsPackageDrugRepository = sharedItemsPackageDrugRepository;
            _validationEngine = validationEngine;
        }
        public async Task<bool> Handle(DeleteDrugItemCommand request, CancellationToken cancellationToken)
        {
            var sharedItemsPackageDrug = await SharedItemsPackageDrug.Get(request.SharedItemsPackageDrugId, _sharedItemsPackageDrugRepository);
            sharedItemsPackageDrug.SetIsDeleted(true);
            return await sharedItemsPackageDrug.Update(_sharedItemsPackageDrugRepository, _validationEngine);
        }
    }
}
