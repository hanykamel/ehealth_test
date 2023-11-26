using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.SharedItemsPackages.SharedItemsPackageCompomnent.Commands
{
    public class DeleteDrugItemCommand :IRequest<bool>
    {
        public DeleteDrugItemCommand(Guid sharedItemsPackageDrugId)
        {
            SharedItemsPackageDrugId=sharedItemsPackageDrugId;
        }
        public Guid SharedItemsPackageDrugId;
    }
}
